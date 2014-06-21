using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TCO
{
    class LegalExploder
    {
        private string strLegal;
        private string[,] quarterNames;

        public LegalExploder(string s)
        {
            strLegal = s;
        }

        public string[] ExplodeToArray()
        {
            List<string> legalOut = new List<string>();
            string[] legalList = strLegal.Split(',');
            for (int i = 0; i < legalList.Length; i++)
            {
                legalOut.AddRange(ExplodeSingleArray(legalList[i]));
            }
            return legalOut.ToArray();
        }

        private string[] ExplodeSingleArray(string s)
        {
            bool[,] landGrid = new bool[4, 4];
            Stack divs = new Stack();
            Stack quarters = new Stack();
            List<string> sbLegalOut = new List<string>();

            // parse legal
            int strLen = s.Length;
            for (int i = 0; i < strLen; i++)
            {
                if (s.Substring(i, 1) == "/")
                {
                    divs.Push(s.Substring(i + 1, 1));
                    // half or quarter?
                    if ((string)divs.Peek() == "2")
                    {
                        quarters.Push(s.Substring(i - 1, 1));
                    }
                    else if ((string)divs.Peek() == "4")
                    {
                        quarters.Push(s.Substring(i - 2, 2));
                    }
                }
            }
            // call gridCalc now
            gridCalc(landGrid, 0, 0, 4, 4, divs, quarters);
            quarterNames = new String[4, 4];
            initializeQuarterNames(0, 0, 4, 4, new Stack());

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (landGrid[i, j] == true)
                    {
                        sbLegalOut.Add(quarterNames[i, j]);
                    }
                }
            }
            return sbLegalOut.ToArray();
        }

        public string Explode()
        {
            StringBuilder legalOut = new StringBuilder();
            string[] legalList = strLegal.Split(',');
            for (int i = 0; i < legalList.Length; i++)
            {
                legalOut.Append(ExplodeSingle(legalList[i].Trim()));
            }
            //return legalOut.ToString();
            return '(' + legalOut.ToString().Trim().Replace(' ', ',') + ')';
        }

        private string ExplodeSingle(string s)
        {
            bool[,] landGrid = new bool[4, 4];
            Stack divs = new Stack();
            Stack quarters = new Stack();
            StringBuilder sbLegalOut = new StringBuilder();

            // parse legal
            int strLen = s.Length;
            for (int i = 0; i < strLen; i++)
            {
                if (s.Substring(i, 1) == "/")
                {
                    divs.Push(s.Substring(i + 1, 1));
                    // half or quarter?
                    if ((string)divs.Peek() == "2")
                    {
                        quarters.Push(s.Substring(i - 1, 1));
                    }
                    else if ((string)divs.Peek() == "4")
                    {
                        quarters.Push(s.Substring(i - 2, 2));
                    }
                }
            }
            // call gridCalc now
            gridCalc(landGrid, 0, 0, 4, 4, divs, quarters);
            quarterNames = new String[4, 4];
            initializeQuarterNames(0, 0, 4, 4, new Stack());

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (landGrid[i, j] == true)
                    {
                        sbLegalOut.AppendFormat(" '{0}'", quarterNames[i, j]);
                    }
                }
            }
            //return '(' + sbLegalOut.ToString().Trim().Replace(' ',',') + ')';
            return sbLegalOut.ToString();
        }

        private void gridCalc(bool[,] grid, int orgX, int orgY, int lenX, int lenY, Stack divs, Stack quarters)
        {
            //STRATEGY:  examine the top elements of the stacks divs and quarters
            // call recursively until empty
            // Based upon these values, call recursively, but shift the length and origin accordingly

            // do we have work to do?  If so process, if not we're done
            if (divs.Count != 0 && quarters.Count != 0)
            {

                int norgX = -1, norgY = -1, nlenX = -1, nlenY = -1;
                string curQuarter = (string)quarters.Pop();
                int curDiv = Int32.Parse((string)divs.Pop());

                // set current subGrid according to values
                if (curDiv == 2)
                {
                    if (curQuarter == "N")
                    {
                        norgX = orgX;
                        norgY = orgY + lenY / 2;
                        nlenX = lenX;
                        nlenY = lenY / 2;
                    }
                    else if (curQuarter == "S")
                    {
                        norgX = orgX;
                        norgY = orgY;
                        nlenX = lenX;
                        nlenY = lenY / 2;
                    }
                    else if (curQuarter == "W")
                    {
                        norgX = orgX;
                        norgY = orgY;
                        nlenY = lenY;
                        nlenX = lenX / 2;
                    }
                    else if (curQuarter == "E")
                    {
                        norgX = orgX + lenX / 2;
                        norgY = orgY;
                        nlenX = lenX / 2;
                        nlenY = lenY;
                    }
                }
                else if (curDiv == 4)
                {
                    if (curQuarter == "NW")
                    {
                        norgX = orgX;
                        norgY = orgY + lenY / 2;
                        nlenX = lenX / 2;
                        nlenY = lenY / 2;
                    }
                    else if (curQuarter == "SW")
                    {
                        norgX = orgX;
                        norgY = orgY;
                        nlenX = lenX / 2;
                        nlenY = lenY / 2;
                    }
                    else if (curQuarter == "NE")
                    {
                        norgX = orgX + lenX / 2;
                        norgY = orgY + lenY / 2;
                        nlenX = lenX / 2;
                        nlenY = lenY / 2;
                    }
                    else if (curQuarter == "SE")
                    {
                        norgX = orgX + lenX / 2;
                        norgY = orgY;
                        nlenX = lenX / 2;
                        nlenY = lenY / 2;
                    }
                }
                gridCalc(grid, norgX, norgY, nlenX, nlenY, divs, quarters);
            }
            else
            {
                SetGrid(grid, orgX, orgY, lenX, lenY);
            }
        }

        private void initializeQuarterNames(int orgX, int orgY, int lenX, int lenY, Stack quarters)
        {
            if (lenX == 1 && lenY == 1)
            {
                StringBuilder sb = new StringBuilder();
                while (quarters.Count > 0)
                {
                    sb.Append((string)quarters.Pop());
                }
                quarterNames[orgX, orgY] = sb.ToString();
            }
            else
            {
                Stack nw = new Stack(quarters);
                Stack ne = new Stack(quarters);
                Stack sw = new Stack(quarters);
                Stack se = new Stack(quarters);

                nw.Push("NW");
                ne.Push("NE");
                sw.Push("SW");
                se.Push("SE");

                lenX /= 2;
                lenY /= 2;
                initializeQuarterNames(orgX, orgY + lenY, lenX, lenY, nw);
                initializeQuarterNames(orgX + lenX, orgY + lenY, lenX, lenY, ne);
                initializeQuarterNames(orgX, orgY, lenX, lenY, sw);
                initializeQuarterNames(orgX + lenX, orgY, lenX, lenY, se);
            }
        }

        private void SetGrid(bool[,] grid, int orgX, int orgY, int lenX, int lenY)
        {
            for (int i = orgX; i < orgX + lenX; i++)
            {
                for (int j = orgY; j < orgY + lenY; j++)
                {
                    grid[i, j] = true;
                }
            }
        }
    }
}
