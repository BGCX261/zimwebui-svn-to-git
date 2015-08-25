<%@ Register Assembly="ZimWeb.Web.UI" Namespace="ZimWeb.Web.UI" TagPrefix="zw" %>
<%@ Register TagPrefix="sfCntl" Namespace="Telerik.Cms.Web.UI" Assembly="Telerik.Cms.Web.UI" %>
<sfCntl:CssFileLink runat="server" ID="css1" FileName="~/Customised/Sitefinity/Admin/ControlTemplates/ControlEditor/GoogleMapDesigner.css" />
<div class="ctrlProps">
    <div class="ctrlContent">
        <div class="ctrlContentImage">
            <div class="ctrlContentImageColumn">
                <p>
                    <asp:Label ID="Label4" AssociatedControlID="GoogleKey" runat="server" Text='<%$Resources:GoogleKey %>'></asp:Label>
                    <asp:TextBox ID="GoogleKey" runat="server" CssClass="txt"></asp:TextBox>
                    <em><a href="http://code.google.com/apis/maps/signup.html" target="_blank">
                        <asp:Literal ID="Literal1" runat="server" Text='<%$Resources:GetGoogleKey %>'></asp:Literal></a></em>
                </p>
                <p>
                    <asp:Label ID="Label1" AssociatedControlID="Heading" runat="server" Text='<%$Resources:Heading %>'></asp:Label>
                    <asp:TextBox ID="Heading" runat="server" CssClass="txt"></asp:TextBox>
                </p>
                <p>
                    <asp:Label ID="Label2" AssociatedControlID="Address" runat="server" Text='<%$Resources:Address %>'></asp:Label>
                    <asp:TextBox ID="Address" TextMode="MultiLine" runat="server" CssClass="txt"></asp:TextBox>
                </p>
                <p>
                    <asp:Label ID="Label3" AssociatedControlID="Zoom" runat="server" Text='<%$Resources:Zoom %>'></asp:Label>
                    <asp:DropDownList ID="Zoom" runat="server">
                    </asp:DropDownList>
                </p>
                <p>
                    <asp:Label ID="Label5" AssociatedControlID="EnableDirections" runat="server" Text='<%$Resources:EnableDirections %>'></asp:Label>
                    <asp:CheckBox ID="EnableDirections" runat="server">
                    </asp:CheckBox>
                </p>
                <asp:LinkButton ID="UpdateMap" runat="server" CssClass="CmsButLeft light">
                    <strong class="CmsButRight light">
                        <asp:Literal ID="Literal3" runat="server" Text='<%$Resources:UpdateMap %>'>
                        </asp:Literal></strong></asp:LinkButton>
            </div>
            <div class="ctrlContentImageColumn">
                <zw:GoogleMap runat="server" ID="MapPreview"></zw:GoogleMap>
            </div>
        </div>
    </div>
</div>
