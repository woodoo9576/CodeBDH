using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBDH.Display.Models
{
    public class OnCall
    {
        /// <summary>
        /// xml de BASTARIH olarak belirtilen alan ile BASSAAT string alanı birleşiminin DateTime tipine dönüştürülmüş hali 
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// SURE
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// SERVIS
        /// </summary>
        public string ClinicName { get; set; }
        /// <summary>
        /// ACIKLAMA
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// PERSONEL
        /// </summary>
        public string WatchMan { get; set; }
        /// <summary>
        /// UNVAN
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// NOBETTURKOD
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// NOBETTURAD
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// DETAYACIKLAMA
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// DETAYTUR
        /// </summary>
        public string DetailType { get; set; }

    }
}
