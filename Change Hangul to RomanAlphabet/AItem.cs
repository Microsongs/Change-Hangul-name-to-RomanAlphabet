using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change_Hangul_to_RomanAlphabet
{
    public class AItem
    {
        string name;
        int score;

        public AItem()
        {

        }
        public string Name
        {
            get { return name;}
            set { name = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public void ShowInfo()
        {
            Debug.WriteLine("Name : " + name);
            Debug.WriteLine("Score : " + score);
        }
    }
}
