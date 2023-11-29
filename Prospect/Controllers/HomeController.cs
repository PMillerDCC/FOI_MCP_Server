using DCC_Standard;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class HomeController : DCC_Controller
    {
     
        #region Home Page

        [Authorize]
        public ActionResult Index()
        {
            #region Sandpit


            //string logonID = DCC_Session.CurrentUser.LogonID;

            //Specification specification = new Specification(DCC_Session.AutoDatabaseConnection, 1809);
            //SpecificationList specificationList = new SpecificationList(DCC_Session.AutoDatabaseConnection);
            //Tender tender = new Tender(DCC_Session.AutoDatabaseConnection, 9803);
            //TenderList tenderList = new TenderList(DCC_Session.AutoDatabaseConnection, 1809);
            //Contractor contractor = new Contractor(DCC_Session.AutoDatabaseConnection, 759);
            //ContractorList contractorList = new ContractorList(DCC_Session.AutoDatabaseConnection);

            //Agreement agreement = new Agreement(DCC_Session.AutoDatabaseConnection, 1);
            //AgreementList agreementList = new AgreementList(DCC_Session.AutoDatabaseConnection);
            //Area area = new Area(DCC_Session.AutoDatabaseConnection, 1);
            //AreaList areaList = new AreaList(DCC_Session.AutoDatabaseConnection);
            //ContractType contractType = new ContractType(DCC_Session.AutoDatabaseConnection, 1);
            //ContractTypeList contractTypeList = new ContractTypeList(DCC_Session.AutoDatabaseConnection);
            //LicenceStatus licenceStatus = new LicenceStatus(DCC_Session.AutoDatabaseConnection, 1);
            //LicenceStatusList licenceStatusList = new LicenceStatusList(DCC_Session.AutoDatabaseConnection);
            //LicenceType licenceType = new LicenceType(DCC_Session.AutoDatabaseConnection, 1);
            //LicenceTypeList licenceTypeList = new LicenceTypeList(DCC_Session.AutoDatabaseConnection);
            //LocalAuthority localAuthority = new LocalAuthority(DCC_Session.AutoDatabaseConnection, 1);
            //LocalAuthorityList localAuthorityList = new LocalAuthorityList(DCC_Session.AutoDatabaseConnection);
            //ServiceType serviceType = new ServiceType(DCC_Session.AutoDatabaseConnection, 1);
            //ServiceTypeList serviceTypeList = new ServiceTypeList(DCC_Session.AutoDatabaseConnection);
            //Restriction restriction = new Restriction(DCC_Session.AutoDatabaseConnection, 1);
            //RestrictionList restrictionList = new RestrictionList(DCC_Session.AutoDatabaseConnection);
            #endregion

            ViewBag.PageTitle = "Home Page";
            return View();
        }

        #endregion


        #region About Page

        [Authorize]
        [OutputCache(Duration = 86400)]
        public ActionResult About()
        {
            ViewBag.PageTitle = "About";
            return View();
        }


        [Authorize]
        [OutputCache(Duration = 86400)]
        public ActionResult ICT_ServiceDeskPortal()
        {
            Response.Redirect("http://itlanding.wp-sites.durham.gov.uk/");
            return View();
        }


        #endregion


        #region TimedOut Page

        [OutputCache(Duration = 86400)]
        public ActionResult TimedOut()
        {
            return View();
        }

        #endregion

    }

}
