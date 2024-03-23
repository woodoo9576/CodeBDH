using System;

namespace CodeBDH.Display.Models
{
    
    public class Line
    {
        /// <summary>
        /// Serviskeyi Line Id olarak belirlendi
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// Polikliniğin Adı
        /// </summary>
        public string Clinic { get; set; }

        /// <summary>
        /// Doktor Adı
        /// </summary>
        public string Doctor { get; set; }

        /// <summary>
        /// Hasta Adı
        /// </summary>
        public string Patient { get; set; }

        /// <summary>
        /// Sıra Numarası
        /// </summary>
        public string LineNumber { get; set; }

        /// <summary>
        /// Poliklinik de durum Poliklinikte,Ameliyatta vs.
        /// </summary>
        public string ClinicState { get; set; }

        /// <summary>
        /// Banko durumu ile ilgili bir durum
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Toplam sıra alan hasta sayısı
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// muayene edilmiş hasta sayısı
        /// </summary>
        public int? Examined { get; set; }

        public int? LeftOver
        {
            get
            {
                return Total - Examined;
            }
        }
        public int? Percent
        {
            get
            {
                return (int)Math.Round((decimal)(Examined == null || Examined == 0 ? 0 : Examined * 100 / Total));
            }
        }
    }
}