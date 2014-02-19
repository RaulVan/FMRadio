using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace FMRaidoLoca.Date
{
    public class RadioList
    {
        [XmlElement("name")]
        public string RadioName { get; set; }

        [XmlElement("frequency")]
        public double RadioFrequency { get; set; }

        [XmlElement("area")]
        public string Area { get; set; }

        [XmlElement("group")]
        public string Group { get; set; }
    }
}
