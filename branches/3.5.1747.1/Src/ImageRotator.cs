using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Cms.Web.UI;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;
using Telerik.Cms.Engine;
using Telerik.Libraries;
using System.Web;
using Telerik;
using System.Collections;
using Telerik.Framework.Web.Design;
using Telerik.Framework.Web;

namespace ZimWeb.Web.UI
{
    [ControlDesigner("ZimWeb.Web.UI.Design.ImageRotatorControlDesigner")]
    class ImageRotator : CompositeControl, IEmptyControl
    {
        #region Fields
        private Container container;
        private string templatePath = "~/Customised/Sitefinity/ControlTemplates/ImageRotator/SimpleRotator.ascx";
        private ITemplate template;
        private string items;
        private LibraryManager manager;
        private IList dataSource;
        #endregion

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            InitializeTemplate();           

            this.container.Rotator.DataSource = this.CustomDataSource;
            this.container.Rotator.DataBind();

            this.Controls.Add(this.container);
        }

        protected virtual void InitializeTemplate()
        {
            container = new Container(this);
            template = ControlUtils.GetTemplate<DefaultTemplate>(this.TemplatePath);
            this.template.InstantiateIn(this.container);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {

        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {

        }

        #region Properties

        private IList CustomDataSource
        {
            get
            {
                if (dataSource == null)
                {
                    List<RotatorImage> images = new List<RotatorImage>();
                    if (!string.IsNullOrEmpty(this.Items))
                    {
                        string[] itemData = this.Items.Split(';');


                        for (int index = 0; index < itemData.Length; index++)
                        {
                            Guid id = new Guid(itemData[index]);
                            IContent content = Manager.GetContent(id);

                            RotatorImage img = new RotatorImage();
                            img.Height = (long)content.GetMetaData("Height");
                            img.Width = (long)content.GetMetaData("Width");
                            img.AlternateText = content.GetMetaData("AlternateText").ToString();
                            img.Url = content.Url;

                            images.Add(img);
                        }
                    }
                    dataSource = images;
                }
                return dataSource;
            }
        }

        public string TemplatePath
        {
            get
            {
                return this.templatePath;
            }
            set
            {
                this.templatePath = value;
            }
        }

        public string Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }        

        private LibraryManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new LibraryManager();
                return manager;
            }
        }

        #endregion

        #region Nested Types

        private class Container : GenericContainer<ImageRotator>
        {
            public Container(ImageRotator owner) : base(owner) { }

            #region Fields
            private RadRotator rotator;
            #endregion

            #region Properties
            public RadRotator Rotator
            {
                get
                {
                    if (rotator == null)
                        rotator = this.FindRequiredControl<RadRotator>();
                    return rotator;
                }
            }
            #endregion
        }

        private class DefaultTemplate : ITemplate
        {
            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                RadRotator rotator = new RadRotator();
                rotator.ID = "rotator";
                container.Controls.Add(rotator);
            }

            #endregion
        }


        #endregion

        #region IEmptyControl Members

        public bool IsEmpty
        {
            get
            {
                return (this.CustomDataSource.Count == 0);
            }
        }

        public string SetEmptyControlDefaultMessage()
        {
            if (this.CustomDataSource.Count == 0)
            {
                return "Please select some pictures in the rotator";
            }

            return string.Empty;
        }

        #endregion
    }
}
