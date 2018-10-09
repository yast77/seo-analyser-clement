using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEOAnalyser
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitializeControl();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                lblResults.Visible = false;

                if (!ValidateInput())
                {
                    lblResults.Visible = true;
                    InitializeControl();
                    return;
                }

                //Check if option Analyse English is checked
                if (cbAnalyseEnglish.Checked)
                {
                    AnalyseEnglishHandler();
                }

                //Check if option Analyse Url is checked
                if (cbAnalyseUrl.Checked)
                {
                    AnalyseUrlHandler();
                }
            }
            catch (WebException we)
            {
                lblResults.Visible = true;
                lblResults.Text = string.Format("There is problem when access to remote server, please try again. Error message: {0}", we.Message);
                return;
            }
            catch (Exception ex)
            {
                lblResults.Visible = true;
                lblResults.Text = string.Format("Sorry, an unexpected error occurred. Error message: {0}", ex.Message);
                return;
            }
        }

        private void AnalyseUrlHandler()
        {
            var client = new WebClient();
            string downloadString = client.DownloadString(tbInput.Text.Trim());

            //Get number of occurrences on the page of each word listed in meta tags
            var getNumberOfMetaTagOccurrence = ProcessInput.GetNumberOfMetatagOccurrence(downloadString);

            gvOccurrenceMetaTagResults.DataSource = getNumberOfMetaTagOccurrence;
            gvOccurrenceMetaTagResults.DataBind();

            ViewState["dsmetatagoccurrencepage"] = getNumberOfMetaTagOccurrence;
            ViewState["sortdsmetatagoccurrencepage"] = "Asc";

            //Get number of occurrences on the page of each word
            var getNumberOfOccurrenceOnPage = ProcessInput.GetNumberOfOccurrencePage(ProcessInput.ConvertHtmlToString(downloadString));

            gvOccurrenceWordResults.DataSource = getNumberOfOccurrenceOnPage;
            gvOccurrenceWordResults.DataBind();

            ViewState["dsnumberofoccurrencepage"] = getNumberOfOccurrenceOnPage;
            ViewState["sortdsnumberofoccurrencepage"] = "Asc";

            //Get number of external links in the page
            var lExternalLinks = ProcessInput.GetExternalLinkList(downloadString);

            lblTotalExternalLinks.Text = lExternalLinks.Sum(x => x.OccurrenceCount).ToString();

            gvExternalLinksResults.DataSource = lExternalLinks;
            gvExternalLinksResults.DataBind();

            ViewState["dsexternallinkpage"] = lExternalLinks;
            ViewState["sortdsexternallinkpage"] = "Asc";
        }

        private void AnalyseEnglishHandler()
        {
            //Get number of occurrences on the page of each word
            var getNumberOfOccurrenceOnText = ProcessInput.GetNumberOfOccurrencePage(tbInput.Text.Trim());

            gvOccurrenceWordResults.DataSource = getNumberOfOccurrenceOnText;
            gvOccurrenceWordResults.DataBind();

            ViewState["dsnumberofoccurrencepage"] = getNumberOfOccurrenceOnText;
            ViewState["sortdsnumberofoccurrencepage"] = "Asc";

            //Get number of external links in the text
            var lExternalLinks = ProcessInput.GetExternalLinkList(tbInput.Text.Trim());

            lblTotalExternalLinks.Text = lExternalLinks.Sum(x => x.OccurrenceCount).ToString();

            gvExternalLinksResults.DataSource = lExternalLinks;
            gvExternalLinksResults.DataBind();

            ViewState["dsexternallinkpage"] = lExternalLinks;
            ViewState["sortdsexternallinkpage"] = "Asc";

            gvOccurrenceMetaTagResults.DataSource = null;
            gvOccurrenceMetaTagResults.DataBind();
        }

        private void InitializeControl()
        {
            gvOccurrenceWordResults.DataSource = null;
            gvOccurrenceWordResults.DataBind();

            gvExternalLinksResults.DataSource = null;
            gvExternalLinksResults.DataBind();

            gvOccurrenceMetaTagResults.DataSource = null;
            gvOccurrenceMetaTagResults.DataBind();

            lblTotalExternalLinks.Text = "0";
        }

        private bool ValidateInput()
        {
            var inputText = tbInput.Text.Trim();
            //Check if none of the option is checked
            if (!cbAnalyseEnglish.Checked && !cbAnalyseUrl.Checked)
            {
                lblResults.Text = "Please select any one of the analyse option.";
                return false;
            }

            //Check if both option is checked
            if (cbAnalyseEnglish.Checked && cbAnalyseUrl.Checked)
            {
                lblResults.Text = "Please select only one of the analyse option.";
                return false;
            }

            //Check if the input is empty or not
            if (string.IsNullOrWhiteSpace(inputText))
            {
                lblResults.Text = "Input cannot be empty.";
                return false;
            }

            //Check if Analyse Url option is checked, only return true when there is one valid url only.
            if (cbAnalyseUrl.Checked)
            {
                var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var lExternalList = linkParser.Matches(inputText).Cast<Match>().Select(m => m.Value).ToList();

                //Check if input text consist more than 1 url
                if (lExternalList.Count() > 1)
                {
                    lblResults.Text = "Input Url is more than 1.";
                    return false;
                }

                //Check if the url of input text is valid
                if (!Uri.IsWellFormedUriString(inputText, UriKind.Absolute))
                {
                    lblResults.Text = "Input Url is not valid.";
                    return false;
                }
            }

            return true;
        }

        protected void gvOccurrenceWordResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                var dtresult = (List<OccurrenceModel>)ViewState["dsnumberofoccurrencepage"];
                if (dtresult.Count() > 0)
                {
                    if (Convert.ToString(ViewState["sortdsnumberofoccurrencepage"]) == "Asc")
                    {
                        dtresult = dtresult.OrderByDescending(x => x.GetType().GetProperty(e.SortExpression).GetValue(x, null)).ToList();
                        ViewState["sortdsnumberofoccurrencepage"] = "Desc";
                    }
                    else
                    {
                        dtresult = dtresult.OrderBy(x => x.GetType().GetProperty(e.SortExpression).GetValue(x, null)).ToList();
                        ViewState["sortdsnumberofoccurrencepage"] = "Asc";
                    }
                    gvOccurrenceWordResults.DataSource = dtresult;
                    gvOccurrenceWordResults.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblResults.Visible = true;
                lblResults.Text = string.Format("Sorry, there is error when doing sorting. Error message: {0}", ex.Message);
            }
        }

        protected void gvOccurrenceMetaTagResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                var dtresult = (List<OccurrenceModel>)ViewState["dsmetatagoccurrencepage"];
                if (dtresult.Count() > 0)
                {
                    if (Convert.ToString(ViewState["sortdsmetatagoccurrencepage"]) == "Asc")
                    {
                        dtresult = dtresult.OrderByDescending(x => x.GetType().GetProperty(e.SortExpression).GetValue(x, null)).ToList();
                        ViewState["sortdsmetatagoccurrencepage"] = "Desc";
                    }
                    else
                    {
                        dtresult = dtresult.OrderBy(x => x.GetType().GetProperty(e.SortExpression).GetValue(x, null)).ToList();
                        ViewState["sortdsmetatagoccurrencepage"] = "Asc";
                    }
                    gvOccurrenceMetaTagResults.DataSource = dtresult;
                    gvOccurrenceMetaTagResults.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblResults.Visible = true;
                lblResults.Text = string.Format("Sorry, there is error when doing sorting. Error message: {0}", ex.Message);
            }

        }

        protected void gvExternalLinksResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                var dtresult = (List<OccurrenceModel>)ViewState["dsexternallinkpage"];
                if (dtresult.Count() > 0)
                {
                    if (Convert.ToString(ViewState["sortdsexternallinkpage"]) == "Asc")
                    {
                        dtresult = dtresult.OrderByDescending(x => x.GetType().GetProperty(e.SortExpression).GetValue(x, null)).ToList();
                        ViewState["sortdsexternallinkpage"] = "Desc";
                    }
                    else
                    {
                        dtresult = dtresult.OrderBy(x => x.GetType().GetProperty(e.SortExpression).GetValue(x, null)).ToList();
                        ViewState["sortdsexternallinkpage"] = "Asc";
                    }
                    gvExternalLinksResults.DataSource = dtresult;
                    gvExternalLinksResults.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblResults.Visible = true;
                lblResults.Text = string.Format("Sorry, there is error when doing sorting. Error message: {0}", ex.Message);
            }

        }
    }
}