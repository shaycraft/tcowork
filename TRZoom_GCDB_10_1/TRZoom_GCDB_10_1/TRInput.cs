using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using System.Text.RegularExpressions;

namespace TRZoom_GCDB_10_1
{
    public class TRInput : ESRI.ArcGIS.Desktop.AddIns.ComboBox
    {
        public TRInput()
        {
        }

        #region "Select Map Features by Attribute Query"

        ///<summary>Select features in the IActiveView by an attribute query using a SQL syntax in a where clause.</summary>
        /// 
        ///<param name="activeView">An IActiveView interface</param>
        ///<param name="featureLayer">An IFeatureLayer interface to select upon</param>
        ///<param name="whereClause">A System.String that is the SQL where clause syntax to select features. Example: "CityName = 'Redlands'"</param>
        ///  
        ///<remarks>Providing and empty string "" will return all records.</remarks>
        public void SelectMapFeaturesByAttributeQuery(IActiveView activeView, IFeatureLayer featureLayer, System.String whereClause)
        {
            if (activeView == null || featureLayer == null || whereClause == null)
            {
                return;
            }
            IFeatureSelection featureSelection = featureLayer as IFeatureSelection; // Dynamic Cast

            // Set up the query
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;

            // Invalidate only the selection cache. Flag the original selection
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            // Perform the selection
            featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);


            // Flag the new selection
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
        }
        #endregion

        #region "Perform Attribute Query"

        ///<summary>Creates an attribute query based on a supplied table and where clause.</summary>
        ///  
        ///<param name="table">An ESRI.ArcGIS.Geodatabase.ITable, this could be a table or feature class</param>
        ///<param name="whereClause">A System.String, (e.g., "city_name = 'Redlands'").</param>
        ///   
        ///<returns>An ICursor holding the results of the query will be returned.</returns>
        ///   
        ///<remarks>The SQL syntax used to specify the where clause is the same as that of the underlying database holding the data. If you would like to return everything in the table supply "" for the where clause.</remarks>
        public ICursor PerformAttributeQuery(ITable table, System.String whereClause)
        {
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause; // create the where clause statement

            // query the table passed into the function and use a cursor to hold the results
            ICursor cursor = table.Search(queryFilter, false);

            return cursor;
        }
        #endregion

        // get index number for layer name
        public System.Int32 GetIndexNumberFromLayerName(ESRI.ArcGIS.Carto.IActiveView activeView, System.String layerName)
        {
            if (activeView == null || layerName == null)
            {
                return -1;
            }
            ESRI.ArcGIS.Carto.IMap map = activeView.FocusMap;

            // Get the number of layers
            int numberOfLayers = map.LayerCount;

            // Loop through the layers and get the correct layer index
            for (System.Int32 i = 0; i < numberOfLayers; i++)
            {
                if (layerName == map.get_Layer(i).Name)
                {

                    // Layer was found
                    return i;
                }
            }

            // No layer was found
            return -1;
        }

        private ILayer getLayerByName(string name)
        {
            UID pid = new UID();
            pid.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";

            ESRI.ArcGIS.Carto.IMap map = ArcMap.Document.ActiveView.FocusMap;
            IEnumLayer layers = map.get_Layers(pid, true);

            ILayer curLayer = layers.Next();
            while (curLayer != null)
            {
                //System.Windows.Forms.MessageBox.Show("curLayer name is " + curLayer.Name);
                if (curLayer.Name == name)
                {
                    return curLayer;
                }
                curLayer = layers.Next();
            }

            return null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

        protected override void OnEnter()
        {
            ILayer sectionLayer = getLayerByName("Section");

            string strInput = Value.Trim();

            Regex _regex = new Regex(@"^([0-9]+)([nNsS])-([0-9]+)([eEwW]),([0-9]+)$");
            Regex _townRegex = new Regex(@"^([0-9]+)([nNsS])-([0-9]+)([eEwW])$");


            if (_regex.IsMatch(strInput))
            {

                GroupCollection groups = _regex.Match(strInput).Groups;
                string strTownship = groups[1].ToString().PadLeft(3, '0');
                string strTownshipDir = groups[2].ToString();
                string strRange = groups[3].ToString().PadLeft(3, '0');
                string strRangeDir = groups[4].ToString();
                string strSection = groups[5].ToString().PadLeft(2, '0');


                StringBuilder strWhereClause = new StringBuilder();
                strWhereClause.AppendFormat("SUBSTRING(\"FRSTDIVID\",5,3) = '{0}' AND SUBSTRING(\"FRSTDIVID\",9,1) = '{1}' ", strTownship, strTownshipDir);
                strWhereClause.AppendFormat("AND SUBSTRING(\"FRSTDIVID\",10,3) = '{0}' AND SUBSTRING(\"FRSTDIVID\",14,1) = '{1}' ", strRange, strRangeDir);
                strWhereClause.AppendFormat("AND SUBSTRING(\"FRSTDIVID\",18,2) = '{0}'", strSection);


                SelectMapFeaturesByAttributeQuery(ArcMap.Document.ActiveView, (IFeatureLayer)sectionLayer, strWhereClause.ToString());
                ZoomToSelectedFeatures((IFeatureLayer)sectionLayer);

            }
            else if (_townRegex.IsMatch(strInput))
            {
                ILayer townshipLayer = getLayerByName("Township");

                GroupCollection groups = _townRegex.Match(strInput).Groups;
                string strTownship = groups[1].ToString().PadLeft(3, '0');
                string strTownshipDir = groups[2].ToString();
                string strRange = groups[3].ToString().PadLeft(3, '0');
                string strRangeDir = groups[4].ToString();

                StringBuilder strWhereClause = new StringBuilder();
                strWhereClause.AppendFormat("SUBSTRING(\"PLSSID\",5,3) = '{0}' AND SUBSTRING(\"PLSSID\",9,1) = '{1}' ", strTownship, strTownshipDir);
                strWhereClause.AppendFormat("AND SUBSTRING(\"PLSSID\",10,3) = '{0}' AND SUBSTRING(\"PLSSID\",14,1) = '{1}' ", strRange, strRangeDir);

                SelectMapFeaturesByAttributeQuery(ArcMap.Document.ActiveView, (IFeatureLayer)townshipLayer, strWhereClause.ToString());
                ZoomToSelectedFeatures((IFeatureLayer)townshipLayer);
            }
            else
            {
                throw new ApplicationException("Invalid Input");
            }

            base.OnEnter();
        }

        private void ZoomToSelectedFeatures(IFeatureLayer layer)
        {
            IMxDocument mxDocument = (IMxDocument)ArcMap.Application.Document;
            IActiveView activeView = ArcMap.Document.ActiveView;
            IFeatureLayer featureLayer = (IFeatureLayer)layer;
            IEnvelope envelope = activeView.Extent;


            ISpatialFilter spatialFilterCls = new SpatialFilterClass();

            IFeatureSelection featureSelection = (IFeatureSelection)layer;
            ISelectionSet selectionSet = featureSelection.SelectionSet;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            string shapeField = featureClass.ShapeFieldName;
            spatialFilterCls.GeometryField = shapeField;

            ICursor cursor;
            selectionSet.Search(spatialFilterCls, true, out cursor);
            IFeature feature;
            IFeatureCursor featureCursor = (IFeatureCursor)cursor;

            feature = featureCursor.NextFeature();
            //envelope = feature.Extent.Envelope;
            envelope = feature.Shape.Envelope;

            activeView.Extent = envelope;

            //System.Windows.Forms.MessageBox.Show("clearing selection now");
            featureSelection.Clear();



            activeView.Refresh();
            //System.Windows.Forms.MessageBox.Show("cleared");
        }
    }

}
