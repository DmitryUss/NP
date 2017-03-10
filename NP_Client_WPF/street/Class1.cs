using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace street
{
    public class Streets
    {
        public string name { get; set; }
        public int index { get; set; }
        List<Streets> landStreets = new List<Streets>();

        public void AddStreet()
        {
            landStreets.Add(new Streets { name = "Olshevskogo", index = 220073 });
            landStreets.Add(new Streets { name = "Pritickogo", index = 220073 });
            landStreets.Add(new Streets { name = "TestStreet", index = 111111 });
            landStreets.Add(new Streets { name = "Nekrasovo", index = 220040 });
            landStreets.Add(new Streets { name = "Surganovo", index = 220040 });
            landStreets.Add(new Streets { name = "Dostoevskogo", index = 220040 });

        }

        public void ShowStreets()
        {
            foreach (var item in landStreets)
            {
                Console.WriteLine("{0} - {1}", item.name, item.index);
            }
        }

        public List<Streets> Search(int index)
        {
            List<Streets> resultSearch = new List<Streets>();
            foreach (var item in landStreets)
            {
                if (index == item.index)
                {
                    Console.WriteLine(item.name);
                    resultSearch.Add(item);
                }
            }
            return resultSearch;
        }
    }

 
}
