using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xml_work
{
	class Station
	{
		public string locationName {get; set; }
        public string parameterValue1 { get; set; }
        public string parameterValue2 { get; set; }
        public string rainValue { get; set; }
	}
	class Program
	{
		static void Main(string[] args)
		{
            List<Station> station = FindStations();

			ShowStation(station);

			Console.ReadKey();
		}

		public static List<Station> FindStations()
		{
			List<Station> stations = new List<Station>();

            var xml = XElement.Load(@"O-A0002-001.xml");

			XNamespace xmln = @"urn:cwb:gov:tw:cwbcommon:0.1";
            var stationsNode = xml.Descendants(xmln + "location").ToList();
            stationsNode.Where(x => !x.IsEmpty).ToList().ForEach(stationNode =>
            {
				Station station = new Station();
                station.locationName = stationNode.Element(xmln + "locationName").Value;

                int num = 1;
                stationNode.Descendants(xmln + "parameter").ToList().ForEach(parameterNode =>
                {
                    if(num == 1) station.parameterValue1 = parameterNode.Element(xmln + "parameterValue").Value;
                    else if(num == 3) station.parameterValue2 = parameterNode.Element(xmln + "parameterValue").Value;
                    num++;
                });

                num = 1;
                stationNode.Descendants(xmln + "weatherElement").ToList().ForEach(weatherNode =>
                {
                    if (num == 7) station.rainValue = weatherNode.Element(xmln + "elementValue").Element(xmln + "value").Value;
                    num++;
                });
   
				stations.Add(station);
			});
            return stations;
		}
		static void ShowStation(List<Station> stations) 
        {
            Console.WriteLine(string.Format("高雄各地區日累計雨量"));
            Console.WriteLine();
            stations.Where(city => city.parameterValue1 == "高雄市").ToList().ForEach(location =>
            {
                Console.WriteLine(string.Format("地點:  {0}\t{1,-6}\t日累積雨量:{2,8}毫米", location.parameterValue2, location.locationName, location.rainValue));
            });
		}
	}
}
