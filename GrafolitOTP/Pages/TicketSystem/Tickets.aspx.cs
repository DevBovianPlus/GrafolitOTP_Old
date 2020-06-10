using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.TicketSystem
{
    public partial class Tickets : ServerMasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewTickets_DataBinding(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewTickets_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("RecallForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void priorityImage_Init(object sender, EventArgs e)
        {
            ASPxImage image = sender as ASPxImage;
            GridViewDataItemTemplateContainer container = image.NamingContainer as GridViewDataItemTemplateContainer;
            int value = Convert.ToInt32(ASPxGridViewTickets.GetRowValues(container.VisibleIndex, "Prioriteta"));
            image.ImageUrl = value == 0 ? "Red.png" : "Green.png";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewTickets.GetRowValues(ASPxGridViewTickets.FocusedRowIndex, "TicketID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.TicketSession)).Cast<Enums.TicketSession>().ToList());

            RedirectWithCustomURI("TicketForm.aspx", (int)Enums.UserAction.Delete, valueID);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearAllSessions(Enum.GetValues(typeof(Enums.TicketSession)).Cast<Enums.TicketSession>().ToList());
            RedirectWithCustomURI("TicketForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewTickets.GetRowValues(ASPxGridViewTickets.FocusedRowIndex, "TicketID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.TicketSession)).Cast<Enums.TicketSession>().ToList());

            RedirectWithCustomURI("TicketForm.aspx", (int)Enums.UserAction.Edit, valueID);
        }

        protected void ASPxGridViewTickets_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void ASPxGridViewTickets_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

        }
    }
}