using System;
using System.Linq;
using System.Threading.Tasks;
using AltInnSrr.Connected_Services.AltInnSrrService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace AltInnSrr.Test
{
    [TestClass]
    public class SrrClientTests
    {
        private GetRightResponseList getRightResponseList;

        [TestInitialize]
        public void InitializeTest()
        {
            getRightResponseList = new GetRightResponseList();
        }
        
        [TestMethod]
        public async Task GetRights_OneOrganisationsFromAltInn_ReturnsListOfOne()
        {
            var validTo = DateTime.Now.AddYears(2);
            var serviceClient = Substitute.For<IServiceClient>();

            var orgnr = 123456789;
            AddRightForOrg(orgnr, validTo);

            serviceClient.GetAllRights().ReturnsForAnyArgs(getRightResponseList);

            var srrClient = new SrrClient(serviceClient);
            var result = await  srrClient.GetRights();
            var altInnSrrRightses = result as AltInnSrrRights[] ?? result.ToArray();
            Assert.AreEqual(1, altInnSrrRightses.Count());
            Assert.IsTrue(altInnSrrRightses.FirstOrDefault().HasMoveRights());
            Assert.AreEqual(orgnr, altInnSrrRightses.FirstOrDefault().OrgNr);
        }

        [TestMethod]
        public async Task GetRights_TwoOrganisationsFromAltInn_ReturnsListOfTwo()
        {
            var validTo = DateTime.Now.AddYears(2);
            var validTo2 = DateTime.Now.AddYears(1);
            var orgnr = 123456789;
            var orgnr2 = 987654321;

            AddRightForOrg(orgnr, validTo);
            AddRightForOrg(orgnr2, validTo2);

            var serviceClient = Substitute.For<IServiceClient>();
            serviceClient.GetAllRights().ReturnsForAnyArgs(getRightResponseList);

            var srrClient = new SrrClient(serviceClient);
            var result = await  srrClient.GetRights();
            var altInnSrrRightses = result as AltInnSrrRights[] ?? result.ToArray();
            Assert.AreEqual(2, altInnSrrRightses.Count());
            Assert.AreEqual(validTo2, altInnSrrRightses.FirstOrDefault(o => o.OrgNr == orgnr2).ReadRightValidTo);
        }

        [TestMethod]
        public async Task DelteRight_OkReturned_NoExcptionsTrown()
        {
            const int orgnr = 123456789;
            const OperationResult result = OperationResult.Ok;

            var serviceClient = Substitute.For<IServiceClient>();
            var deleteRightsResponseList = new DeleteRightResponseList
            {
                GetDeleteRightItem(orgnr, result, RegisterSRRRightsType.Read),
                GetDeleteRightItem(orgnr, result, RegisterSRRRightsType.Write),

            };

            serviceClient.DeleteRights(Arg.Any<int>()).ReturnsForAnyArgs(deleteRightsResponseList);

            var srrClient = new SrrClient(serviceClient);
            await srrClient.DeleteRights(orgnr);
        }

        [TestMethod]
        [ExpectedException(typeof(AltInnSrrException))]
        public async Task DelteRight_BothRightsNotOk_ThrowsAltInnSrrException()
        {
            const int orgnr = 123456789;
            const OperationResult notOkResult = OperationResult.RuleNotFound;

            var serviceClient = Substitute.For<IServiceClient>();
            var deleteRightsResponseList = new DeleteRightResponseList
            {
                GetDeleteRightItem(orgnr, notOkResult, RegisterSRRRightsType.Read),
                GetDeleteRightItem(orgnr, notOkResult, RegisterSRRRightsType.Write),

            };

            serviceClient.DeleteRights(Arg.Any<int>()).ReturnsForAnyArgs(deleteRightsResponseList);

            var srrClient = new SrrClient(serviceClient);
            await srrClient.DeleteRights(orgnr);
        }

        [TestMethod]
        [ExpectedException(typeof(AltInnSrrException))]
        public async Task DelteRight_ReadRightsNotOk_ThrowsAltInnSrrException()
        {
            const int orgnr = 123456789;
            const OperationResult notOkResult = OperationResult.RuleNotFound;

            var serviceClient = Substitute.For<IServiceClient>();
            var deleteRightsResponseList = new DeleteRightResponseList
            {
                GetDeleteRightItem(orgnr, notOkResult, RegisterSRRRightsType.Read),
                GetDeleteRightItem(orgnr, OperationResult.Ok, RegisterSRRRightsType.Write),

            };

            serviceClient.DeleteRights(Arg.Any<int>()).ReturnsForAnyArgs(deleteRightsResponseList);

            var srrClient = new SrrClient(serviceClient);
            await srrClient.DeleteRights(orgnr);
        }

        [TestMethod]
        public async Task DelteRight_ReadRightsNotOk_ThrowsAltInnSrrExceptionWithMessage()
        {
            const int orgnr = 123456789;
            const string expectedMessage = "Feil ved sletting av rettigheter: Read - RuleNotFound";
            const OperationResult notOkResult = OperationResult.RuleNotFound;

            var serviceClient = Substitute.For<IServiceClient>();
            var deleteRightsResponseList = new DeleteRightResponseList
            {
                GetDeleteRightItem(orgnr, notOkResult, RegisterSRRRightsType.Read),
                GetDeleteRightItem(orgnr, OperationResult.Ok, RegisterSRRRightsType.Write),

            };

            serviceClient.DeleteRights(Arg.Any<int>()).ReturnsForAnyArgs(deleteRightsResponseList);

            var srrClient = new SrrClient(serviceClient);
            try
            {
                await srrClient.DeleteRights(orgnr);
            }
            catch (Exception e)
            { 
                Assert.AreEqual(expectedMessage, e.Message);
            }
            
        }

        [TestMethod]
        [ExpectedException(typeof(AltInnSrrException))]
        public async Task DelteRight_WriteRightsNotOk_ThrowsAltInnSrrException()
        {
            const int orgnr = 123456789;
            const OperationResult notOkResult = OperationResult.RuleNotFound;

            var serviceClient = Substitute.For<IServiceClient>();
            var deleteRightsResponseList = new DeleteRightResponseList
            {
                GetDeleteRightItem(orgnr, OperationResult.Ok, RegisterSRRRightsType.Read),
                GetDeleteRightItem(orgnr, notOkResult, RegisterSRRRightsType.Write),

            };

            serviceClient.DeleteRights(Arg.Any<int>()).ReturnsForAnyArgs(deleteRightsResponseList);

            var srrClient = new SrrClient(serviceClient);
            await srrClient.DeleteRights(orgnr);
        }

        private static DeleteRightResponse GetDeleteRightItem(int orgnr, OperationResult result, RegisterSRRRightsType right)
        {
            return new DeleteRightResponse
            {
                Reportee = orgnr.ToString(),
                OperationResult = result,
                Right = right
            };
        }


        private void AddRightForOrg(int orgnr, DateTime validTo)
        {
            getRightResponseList.Add(new GetRightResponse
            {
                Reportee = orgnr.ToString(),
                Right = RegisterSRRRightsType.Read,
                ValidTo = validTo
            });
            getRightResponseList.Add(new GetRightResponse
            {
                Reportee = orgnr.ToString(),
                Right = RegisterSRRRightsType.Write,
                ValidTo = validTo
            });
        }
    }
}
