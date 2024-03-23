using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Ajax.Utilities;

namespace CodeBDH.Display.DataAccess
{
    using Models;
    public class XmlContext
    {
        private const string XmlSource = @"http://ebys.canakkalekhb.gov.tr/cozumhbys//hastanebilgi?tur=";
        private const string XmlFile = @"http://ebys.canakkalekhb.gov.tr/cozumhbys/hastanebilgi?tur=poliklinikmuayenelistesi&hastaneler=6076";
        //readonly XDocument _doc = new XDocument(XmlFile);

        public IEnumerable<Line> GetLine()
        {
            XDocument doc = XDocument.Load(XmlSource + "poliklinikmuayenelistesi&hastaneler=6076");
            var records = from r in doc.Descendants("ROOT").Elements("RECORDS") select r;
            return records.Select(record => new Line()
            {
                LineId = int.Parse(record.Element("SR_KEY").Value),
                Clinic = record.Element("POLIKLINIK").Value == null ? "-" : record.Element("POLIKLINIK").Value,
                Doctor = record.Element("DOKTOR").Value == "" ? "-" : record.Element("DOKTOR").Value,
                Patient = record.Element("HASTA").Value == "" ? "-" : record.Element("HASTA").Value,
                LineNumber = record.Element("SIRANO").Value == "" ? "0" : record.Element("SIRANO").Value,
                ClinicState = "-",
                State = "I",
                Total = int.Parse(record.Element("MURACAAT").Value),
                Examined = int.Parse(record.Element("MUAYENE").Value)
            }).ToList();

        }

        public IEnumerable<OnCall> GetWatchers()
        {
            XDocument doc = XDocument.Load(XmlSource + "nobetlistesi&sr_key=4035");
            var records = from r in doc.Descendants("ROOT").Elements("RECORDS") select r;
            return records.Select(record => new OnCall()
            {
                StartTime =
                    DateTime.ParseExact(record.Element("BASTARIH").Value + " " + record.Element("BASSAAT").Value,
                        "dd.mm.yyyy HH:mm", CultureInfo.InvariantCulture),
                Duration = int.Parse(record.Element("SURE").Value),
                ClinicName = record.Element("SERVIS").Value,
                Description = record.Element("ACIKLAMA").Value,
                WatchMan = record.Element("PERSONEL").Value,
                Title = record.Element("UNVAN").Value,
                Type = record.Element("NOBETTURKOD").Value,
                TypeName = record.Element("NOBETTURAD").Value,
                Detail = record.Element("DETAYACIKLAMA").Value,
                DetailType = record.Element("DETAYTUR").Value
            }).ToList();
        }

        public Line GetLine(int id)
        {
            var line = GetLine().Single(l => l.LineId == id);
            return line;
        }

        public Line DeSerialize(XElement element)
        {
            var s = new XmlSerializer(typeof(Line));
            var line = (Line)s.Deserialize(new StringReader(element.ToString()));
            return line;
        }
    }

    //public class XmlNobetListesi
    //{
    //    private const string XmlFile = "http://cozumhbys/cozumhbys/hastanebilgi?tur=nobetlistesi&sr_key=4035";

    //    public IEnumerable<OnCall> GetOnCalls()
    //    {
    //        XDocument doc = XDocument.Load(XmlFile);
    //        var records = doc.Descendants("ROOT").Elements("RECORDS").Select(r => r);
    //        return records.Select(record => new OnCall()
    //        {
    //            StartTime = DateTime.ParseExact(record.Element("BASTARIH").Value + " "+ record.Element("BASSAAT").Value , "dd.mm.yyyy HH:mm", CultureInfo.InvariantCulture),
    //            Duration = int.Parse(record.Element("SURE").Value),
    //            ClinicName = record.Element("SERVIS").Value,
    //            Description = record.Element("ACIKLAMA").Value,
    //            WatchMan = record.Element("PERSONEL").Value,
    //            Title = record.Element("UNVAN").Value,
    //            Type = record.Element("NOBETTURKOD").Value,
    //            TypeName = record.Element("NOBETTURAD").Value,
    //            Detail = record.Element("DETAYACIKLAMA").Value,
    //            DetailType = record.Element("DETAYTUR").Value

    //        }).ToList();
    //    }

    //    public IEnumerable<OnCall> GetDocCalls(string type)
    //    {
    //        return GetOnCalls().Where(l => l.Type == type).ToList();
    //    }
    //}
}