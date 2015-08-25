<%@ Register TagPrefix="sfLib" Namespace="Telerik.Libraries.WebControls" Assembly="Telerik.Libraries" %>
<%@ Register TagPrefix="sfWeb" Namespace="Telerik.Cms.Web.UI" Assembly="Telerik.Cms.Web.UI" %>

<telerik:RadCodeBlock ID="CodeBlock1" runat="server">
<script type="text/javascript">
    function setSelected(id, remove) {
        var hiddenSelection = $get(m_hiddenSelection);
        
        if (remove)
            hiddenSelection.value = hiddenSelection.value.replace(id,"");
         else
            hiddenSelection.value += id;
    }
</script>
</telerik:RadCodeBlock>

<div class="ctrlProps">
    <div class="ctrlContent">
        <h3>
            Choose Your Images</h3>
        <asp:DropDownList ID="libraryDropDown" runat="server" DataTextField="Name" DataValueField="ID">
        </asp:DropDownList>
        <asp:Repeater ID="imagesRepeater" runat="server">
            <HeaderTemplate>
                <ul class="availableImages">
            </HeaderTemplate>
            <ItemTemplate>
                <li runat="server" id="itemLi">
                    <asp:CheckBox runat="server" ID="selectCheck" />
                    <sfLib:ItemView ID="contentView" runat="server" ItemTemplatePath="~/Sitefinity/Admin/ControlTemplates/Libraries/SelectorItemView.ascx" />
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:HiddenField runat="server" ID="selectedItems" />
        <sfWeb:Pager runat="server" ID="pager2">
            <LayoutTemplate>
                <asp:Repeater ID="PageRepeaterLinkButton" runat="server">
                    <HeaderTemplate>
                        <ol class="pager">
                            <li>
                                <asp:LinkButton ID="PreviousPage" runat="server" Text="Prev"></asp:LinkButton></li>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:LinkButton ID="SingleItem" runat="server" />
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        <li>
                            <asp:LinkButton ID="NextPage" runat="server" Text="Next"></asp:LinkButton></li>
                        </ol>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="PageRepeaterHyperLink" runat="server">
                    <HeaderTemplate>
                        <ol class="sf_pager">
                            <li>
                                <asp:HyperLink ID="PreviousPage" runat="server" Text="Prev"></asp:HyperLink></li>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="SingleItem" runat="server" />
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        <li>
                            <asp:HyperLink ID="NextPage" runat="server" Text="Next"></asp:HyperLink></li>
                        </ol>
                    </FooterTemplate>
                </asp:Repeater>
            </LayoutTemplate>
        </sfWeb:Pager>
    </div>
</div>
