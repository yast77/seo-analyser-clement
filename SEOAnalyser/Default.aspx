<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SEOAnalyser._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <h2>Input Text in English or URL</h2>
            <asp:TextBox ID="tbInput" runat="server" TextMode="MultiLine" Height="400px" Width="100%"></asp:TextBox>
            <p>
                <asp:CheckBox ID="cbAnalyseEnglish" runat="server" Text="Analyse English Text" />
                <asp:CheckBox ID="cbAnalyseUrl" runat="server" Text="Analyse Url" />
            </p>
            <p>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            </p>
        </div>
        <div class="col-md-12">
            <asp:Label ID="lblResults" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
        </div>
        <div class="col-md-4">
            <h2>Output results - Number of occurrences on the page of each word</h2>
            <p>
                <asp:GridView ID="gvOccurrenceWordResults" runat="server" AllowSorting="True" OnSorting="gvOccurrenceWordResults_Sorting" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Occurrence Word" SortExpression="OccurrenceWordOrLink">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 200px;">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("OccurrenceWordOrLink") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Occurrence Count" SortExpression="OccurrenceCount">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 100px;">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("OccurrenceCount") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Output results - Number of occurrences on the page of each word listed in meta tags</h2>
            <p>
                <asp:GridView ID="gvOccurrenceMetaTagResults" runat="server" AllowSorting="True" AutoGenerateColumns="False" OnSorting="gvOccurrenceMetaTagResults_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Occurrence Word" SortExpression="OccurrenceWordOrLink">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 200px;">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("OccurrenceWordOrLink") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Occurrence Count" SortExpression="OccurrenceCount">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 100px;">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("OccurrenceCount") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Output results - Number of external links in the text</h2>
            <p>
                Total number of external links:
                <asp:Label ID="lblTotalExternalLinks" runat="server" Text=""></asp:Label>
            </p>
            <p>
                <asp:GridView ID="gvExternalLinksResults" runat="server" AllowSorting="True" AutoGenerateColumns="False" OnSorting="gvExternalLinksResults_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="External Links" SortExpression="OccurrenceWordOrLink">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 200px;">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("OccurrenceWordOrLink") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Occurrence Count" SortExpression="OccurrenceCount">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 100px;">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("OccurrenceCount") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </p>
        </div>
    </div>

</asp:Content>
