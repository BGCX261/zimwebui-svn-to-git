using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using Telerik.Cms.Web.UI;
using System.ComponentModel;
using Telerik.Framework.Web;
using Telerik.Web.UI;
using System.Collections;
using System.Collections.Specialized;

namespace ZimWeb.Web.UI.Design
{
    /// <summary>
    /// Summary description for TabstripDesigner
    /// </summary>
    public class EditableTabstripDesigner : Telerik.Framework.Web.Design.ControlDesigner
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the template used by TabstripDesigner control
        /// </summary>
        /// public ITemplate LayoutTemplate
        public override ITemplate LayoutTemplate
        {
            get
            {
                return this.layoutTemplate;
            }
            set
            {
                this.layoutTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets the path of the template used by TabstripDesigner control
        /// </summary>
        /// public virtual string LayoutTemplatePath
        public override string LayoutTemplatePath
        {
            get
            {
                if (this.ViewState["LayoutTemplatePath"] == null)
                    return "~/UserControls/ControlTemplates/EditableTabstrip/Admin/EditableTabstripDesigner.ascx";
                return this.ViewState["LayoutTemplatePath"] as string;
            }
            set
            {
                this.ViewState["LayoutTemplatePath"] = value;
            }
        }

        #endregion

        #region Base Overrides

        /// <summary>
        /// Restores control state information from a previous page request that was saved by the SaveControlState
        /// method.
        /// </summary>
        /// <param name="savedState">Represents the control state to be restored.</param>
        /// <remarks>Notice that this method loads the state from the base class as well</remarks>
        protected override void LoadControlState(object savedState)
        {
            if (savedState != null)
            {
                object[] state = (object[])savedState;
                base.LoadControlState(state[0]);
                this.temporaryListItems = (Dictionary<string, string>)state[1];
            }
        }

        /// <summary>
        /// Saves server control state changes.
        /// </summary>
        /// <returns>Array of objects to be saved with the control state</returns>
        /// <remarks>Notice that this method saves the state for the base class as well</remarks>
        protected override object SaveControlState()
        {
            return new object[] { 
				base.SaveControlState(),
                this.temporaryListItems
			};
        }

        /// <summary>
        /// Renders the HTML opening tag of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //Do not render
        }

        /// <summary>
        /// Renders the HTML closing tag of the control into the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            //Do not render
        }

        /// <summary>
        /// Creates the child controls in EditableTabstripDesigner control.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            this.InitializeTemplate();
            this.InitializeComponent();

            // we need to handle the click on the "Add new item" button
            this.container.AddNewItemButton.Command += new CommandEventHandler(Button_Command);
            this.container.AddNewItemButton.CommandName = "AddNewItem";

            // items of a list that user currently has will be displayed in a repeater
            this.container.ItemsRepeater.ItemDataBound += new RepeaterItemEventHandler(ItemsRepeater_ItemDataBound);
            this.container.ItemsRepeater.ItemCommand += new RepeaterCommandEventHandler(ItemsRepeater_ItemCommand);

            this.Controls.Add(this.container);

            BindItems();

        }

        public override void OnSaving()
        {
            this.component.Tabs = temporaryListItems;
            base.OnSaving();
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Initializes the template to use. The principle is very similar to how we do it in all the controls
        /// </summary>
        protected virtual void InitializeTemplate()
        {
            this.container = new EditableTabstripDesignerContainer(this);
            this.layoutTemplate = ControlUtils.GetTemplate<DefaultEditableTabstripDesignerTemplate>(this.LayoutTemplatePath);
            this.layoutTemplate.InstantiateIn(this.container);
        }

        /// <summary>
        /// Initializes the component which is our public control.
        /// </summary>
        /// <remarks>
        /// By "component" we understand the control for which designer is setting properties. By having a 
        /// reference to the "component" we can access or modify the properties of that control / component.
        /// </remarks>
        protected virtual void InitializeComponent()
        {
            if (base.DesignedControl != null)
            {
                this.component = (IEditableTabstrip)base.DesignedControl;
                this.properties = TypeDescriptor.GetProperties(component);

                this.editorDialog = new PropertyEditorDialog();
                this.editorDialog.TypeContainer = this.component;
                this.editorDialog.PropertyChanged += new PropertyValueChangedEventHandler(this.EditorDialog_PropertyChanged);
                this.Controls.Add(this.editorDialog);
            }

            if (temporaryListItems == null)
                temporaryListItems = component.Tabs;
        }

        /// <summary>
        /// Method in charge of binding the items to the repeater, so that user can see which items are
        /// currently present in the list.
        /// </summary>
        private void BindItems()
        {
            // finally we are binding the items repeater to the temporary list
            this.container.ItemsRepeater.DataSource = temporaryListItems;
            this.container.ItemsRepeater.DataBind();
        }

        /// <summary>
        /// This method adds a new item to the temporary list. The list is called temporary, 
        /// </summary>
        private void AddNewItem()
        {
            temporaryListItems.Add(this.container.ItemTextTextBox.Text, this.container.ContentEditor.Content);
            // every time the new item is added we'll call the UpdateMyItems which will update the changes
            // to the component
            UpdateMyItems();
            BindItems();
        }

        /// <summary>
        /// This method updates an item in the temporary list. The list is called temporary, 
        /// </summary>
        private void UpdateItem()
        {
            temporaryListItems[this.container.ItemTextTextBox.Text] = this.container.ContentEditor.Content;
            // every time the new item is added we'll call the UpdateMyItems which will update the changes
            // to the component
            UpdateMyItems();
            BindItems();
        }


        /// <summary>
        /// This method removes an item from the list.
        /// </summary>
        /// <param name="item">Item in the form of name/value pair as it is stored in the temporary list</param>
        private void RemoveItem(string item)
        {
            if (temporaryListItems.ContainsKey(item))
                temporaryListItems.Remove(item);
            // every time the item is removed we'll call the UpdateMyItems which will update the changes
            // to the component
            UpdateMyItems();
            BindItems();
        }


        /// <summary>
        /// This method loads an item from the list.
        /// </summary>
        /// <param name="item">Item in the form of name/value pair as it is stored in the temporary list</param>
        private void EditItem(string item)
        {
            if (temporaryListItems.ContainsKey(item))
            {
                string value = temporaryListItems[item];
                this.container.ContentEditor.Content = value;
                this.container.ItemTextTextBox.Text = item;
                this.container.ItemTextTextBox.Enabled = false;

                this.container.AddNewItemButton.CommandName = "UpdateItem";
            }
            BindItems();
        }

        /// <summary>
        /// When the MyItems property on the component should be updated, this method is called
        /// </summary>
        protected void UpdateMyItems()
        {
            // we need to notify the base class that property has been changed
            base.OnPropertyChanged(EventArgs.Empty);
        }

        private void CancelEdit()
        {
            this.container.ItemTextTextBox.Text = String.Empty;
            this.container.ContentEditor.Content = String.Empty;
            this.container.ItemTextTextBox.Enabled = true;
        }

        private void MoveItem(string item, MoveDirectionType direction)
        {
            if (temporaryListItems.ContainsKey(item))
            {
                string oldValue = temporaryListItems[item];

                int currentIndex = 0;
                foreach (KeyValuePair<string, string> keyPair in temporaryListItems)
                {
                    //exit if the key is the same
                    if (keyPair.Key == item)
                        break;
                    currentIndex++;
                }

                Dictionary<string, string> newList = new Dictionary<string, string>();
                int i = 0;
                foreach (KeyValuePair<string, string> keyPair in temporaryListItems)
                {
                    if (i == currentIndex - 1 && direction == MoveDirectionType.Up)
                    {
                        newList.Add(item, oldValue);
                    }
                    if (i != currentIndex)
                    {
                        newList.Add(keyPair.Key, keyPair.Value);
                    }
                    if (i == currentIndex + 1 && direction == MoveDirectionType.Down)
                    {
                        newList.Add(item, oldValue);
                    }

                    i++;
                }
                temporaryListItems = newList;
            }

            BindItems();
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the property being changed by the editor.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void EditorDialog_PropertyChanged(object source, PropertyValueChangedEventArgs e)
        {
            PropertyDescriptor descriptor;
            base.SetProperty(this.component, this.properties, e.PropertyName, e.PropertyValue, out descriptor);
            base.OnPropertyChanged(EventArgs.Empty);
            this.RecreateChildControls();
        }

        /// <summary>
        /// Handles basic commands the UI elements on our designer may fire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "AddNewItem":
                    AddNewItem();
                    CancelEdit();
                    break;

                case "UpdateItem":
                    UpdateItem();
                    CancelEdit();
                    break;
            }
        }

        /// <summary>
        /// When item in a repeater is being bound, this method will make sure that
        /// all the properties needed by controls inside of ItemTemplate are set properly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeyValuePair<string, string> itemData = (KeyValuePair<string, string>)e.Item.DataItem;
                ITextControl itemTextLabel = (ITextControl)e.Item.FindControl("itemDetails");
                if (itemTextLabel != null)
                    itemTextLabel.Text = itemData.Key;

                IButtonControl editButton = (IButtonControl)e.Item.FindControl("editButton");
                if (editButton != null)
                {
                    editButton.CommandName = "edit";
                    editButton.CommandArgument = itemData.Key;
                }

                IButtonControl removeItemButton = (IButtonControl)e.Item.FindControl("removeItemButton");
                if (removeItemButton != null)
                {
                    removeItemButton.CommandName = "remove";
                    removeItemButton.CommandArgument = itemData.Key;
                }

                IButtonControl moveUpButton = (IButtonControl)e.Item.FindControl("moveUpButton");
                if (moveUpButton != null)
                {
                    moveUpButton.CommandName = "moveup";
                    moveUpButton.CommandArgument = itemData.Key;
                    ((WebControl)moveUpButton).Enabled = (e.Item.ItemIndex > 0);
                }

                IButtonControl moveDownButton = (IButtonControl)e.Item.FindControl("moveDownButton");
                if (moveDownButton != null)
                {
                    moveDownButton.CommandName = "movedown";
                    moveDownButton.CommandArgument = itemData.Key;
                    ((WebControl)moveDownButton).Enabled = (e.Item.ItemIndex != temporaryListItems.Count - 1);
                }
            }
        }

        /// <summary>
        /// Handles any commands raised by controls inside of repeaters ItemTemplate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsRepeater_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "remove":
                    RemoveItem(e.CommandArgument.ToString());
                    break;

                case "edit":
                    EditItem(e.CommandArgument.ToString());
                    break;

                case "moveup":
                    MoveItem(e.CommandArgument.ToString(), MoveDirectionType.Up);
                    break;

                case "movedown":
                    MoveItem(e.CommandArgument.ToString(), MoveDirectionType.Down);
                    break;
            }
        }

        #endregion

        #region Protected Fields

        /// <summary>
        /// Gets or sets the EditableTabstrip designer container.
        /// </summary>
        /// <value>The EditableTabstrip designer container.</value>
        protected EditableTabstripDesignerContainer Container
        {
            get
            {
                return container;
            }
            set
            {
                container = value;
            }
        }

        /// <summary>
        /// Gets or sets the component which is of IEditableTabstrip type.
        /// </summary>
        /// <value>The component which is of IEditableTabstrip type.</value>
        protected IEditableTabstrip Component
        {
            get
            {
                return component;
            }
            set
            {
                component = value;
            }
        }

        #endregion

        #region Private Fields

        private ITemplate layoutTemplate;
        private EditableTabstripDesignerContainer container;
        private IEditableTabstrip component;
        private PropertyDescriptorCollection properties;
        private Dictionary<string, string> temporaryListItems;
        private PropertyEditorDialog editorDialog;

        #endregion

        #region Enum
        private enum MoveDirectionType
        {
            Up,
            Down
        }

        #endregion

        #region Default Template

        /// <summary>
        /// Default template for the EditableTabstripDesigner control designer. NOT IMPLEMENTED!
        /// </summary>
        protected class DefaultEditableTabstripDesignerTemplate : ITemplate
        {
            /// <summary>
            /// When implemented by a class, defines the object that child controls and templates belong to. These child controls are in turn defined within an inline template.
            /// </summary>
            /// <param name="container">The object to contain the instances of controls from the inline template.</param>
            public void InstantiateIn(Control container)
            {
                throw new NotImplementedException("Default control designer not implemented!");
            }
        }

        #endregion

        #region Container

        /// <summary>
        /// The container class for the EditableTabstripDesigner control designer.
        /// </summary>
        protected class EditableTabstripDesignerContainer : GenericContainer<EditableTabstripDesigner>
        {
            /// <summary>
            /// Initializes a new instance of the EditableTabstripDesignerContainer class.
            /// </summary>
            /// <param name="owner">The EditableTabstripDesigner control.</param>
            public EditableTabstripDesignerContainer(EditableTabstripDesigner owner)
                : base(owner, true)
            {
            }

            public TextBox ItemTextTextBox
            {
                get
                {
                    if (this.itemTextTextBox == null)
                        this.itemTextTextBox = (TextBox)base.FindRequiredControl<Control>("itemTextTextBox");
                    return this.itemTextTextBox;
                }
            }

            public RadEditor ContentEditor
            {
                get
                {
                    if (this.contentEditor == null)
                        this.contentEditor = base.FindRequiredControl<RadEditor>("contentEditor");
                    return this.contentEditor;
                }
            }

            public IButtonControl AddNewItemButton
            {
                get
                {
                    if (this.addNewItemButton == null)
                        this.addNewItemButton = (IButtonControl)base.FindRequiredControl<Control>("addNewItemButton");
                    return this.addNewItemButton;
                }
            }

            public Repeater ItemsRepeater
            {
                get
                {
                    if (this.itemsRepeater == null)
                        this.itemsRepeater = base.FindRequiredControl<Repeater>("itemsRepeater");
                    return this.itemsRepeater;
                }
            }

            private RadEditor contentEditor;
            private TextBox itemTextTextBox;
            private IButtonControl addNewItemButton;
            private Repeater itemsRepeater;
        }

        #endregion

    }

}
