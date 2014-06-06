#include<iostream>
#include<string>
#include<vector>
using namespace std;

void padTo(std::string &str, const size_t num, const char paddingChar = ' ')
{
    if(num > str.size())
	{
        str.insert(0, num - str.size(), paddingChar);
	}
}

void trim(string &str)
{
        string stmp = "";
        int len = str.length();
        for (int i = 0; i < len; i++)
        {
                if (str.compare(i,1, " ")!= 0)
                {
                        //stmp.append(str[i]);
                        stmp.append(str.substr(i,1));
                }
        }

       str=stmp;
}

vector<string> split(const string& strValue, char separator)
{
    vector<string> vecstrResult;
    int startpos=0;
    int endpos=0;

    endpos = strValue.find_first_of(separator, startpos);
    while (endpos != -1)
    {
        vecstrResult.push_back(strValue.substr(startpos, endpos-startpos)); // add to vector
        startpos = endpos+1; //jump past sep
        endpos = strValue.find_first_of(separator, startpos); // find next
        if(endpos==-1)
        {
            //lastone, so no 2nd param required to go to end of string
            vecstrResult.push_back(strValue.substr(startpos));
        }
     }
     return vecstrResult;
}

int main()
{
        string state;
        string township;
        string tdir= "N";
        string range;
        string rdir;
        string meridian;
        string sectionlist;

        cout << "State: ";
		getline(cin,state);
		//trim(state);
        cout << state << endl;

        if (state == "MT")
        {
                meridian = "20";
                rdir="E";
        }
        else if (state == "ND")
        {
                meridian = "05";
                rdir="W";
        }
        else
        {
                cout << "Invalid state\n";
                return 1;
        }

        cout << "Township: ";
        getline(cin, township);
		//trim(township);
        cout << "Range: ";
        getline(cin, range);
		//trim(range);
        cout << "Section list: ";
        getline(cin, sectionlist);
		trim(sectionlist);
        padTo(township,3,'0');
        padTo(range,3,'0');

        vector<string> sectionArray= split(sectionlist, ',');
        if (sectionArray.size() == 0)
        {
                sectionArray.push_back(sectionlist);
        }
        cout << "sectionAarray.size() = " << sectionArray.size() << endl;

        string lndkey="";
        lndkey.append(state);
        lndkey.append(meridian);
        lndkey.append("T");
        lndkey.append(township);
        lndkey.append("0");
        lndkey.append(tdir);
        lndkey.append(range);
        lndkey.append("0");
        lndkey.append(rdir);

        cout << lndkey << endl;

        for (unsigned int i = 0; i < sectionArray.size(); i++)
        {
                string curSection = sectionArray[i];
                padTo(curSection, 3, '0');
                string joinkey = "";
                joinkey.append(lndkey);
                joinkey.append(curSection);
                cout << joinkey << endl;
        }

        return 0;
}