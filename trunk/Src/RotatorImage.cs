using System;
using System.Collections.Generic;
using System.Text;

namespace ZimWeb.Web.UI
{
    public class RotatorImage
    {
        #region Fields
        private string url;
        private string alternateText;
        private long width;
        private long height;
        #endregion

        #region Properties
        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        public string AlternateText
        {
            get
            {
                return this.alternateText;
            }
            set
            {
                this.alternateText = value;
            }
        }

        public long Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public long Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }


        #endregion
    }
}
