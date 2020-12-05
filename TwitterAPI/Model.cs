using System;


namespace TwitterAPI
{
    public class tdata
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class tdatastream
    {
        public DateTime tDate { get; set; }
        public tdata data { get; set; }

        public bool hasEmoji { get; set; } = false;
        public bool hasHashTag { get; set; } = false;
        public bool hasURL { get; set; } = false;

        public bool hasPhotoURL { get; set; } = false;
    }


}
