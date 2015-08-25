<telerik:RadRotator runat="server" ID="rotator">
    <ItemTemplate>
        <asp:Image ID="Image1" ImageUrl='<%# Databinder.Eval(Container,"DataItem.Url").ToString() + ".sflb.ashx" %>' runat="server"  AlternateText='<%# Databinder.Eval(Container,"DataItem.AlternateText") %>' />
    </ItemTemplate>
</telerik:RadRotator>