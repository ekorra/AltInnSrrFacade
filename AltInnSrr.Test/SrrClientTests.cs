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
            Assert.IsTrue(altInnSrrRightses.FirstOrDefault().HasMoveRights);
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

        [TestMethod]
        [ExpectedException(typeof(AltInnSrrException))]
        public async Task AddRights_ExistingOrg_ThrowsExcption()
        {
            const int orgnr = 123456789;
            DateTime validTo = DateTime.Now.AddYears(2);

            var serviceClient = Substitute.For<IServiceClient>();
            var existingValidTo = DateTime.Now.AddYears(1);
            var ruleAlreadyExists = OperationResult.RuleAlreadyExists;

            var addRightsResponseList = new AddRightResponseList
            {
                new AddRightResponse
                {
                    Reportee = orgnr.ToString(),
                    OperationResult = ruleAlreadyExists,
                    Right = RegisterSRRRightsType.Read,
                    ValidTo = existingValidTo
                },
                new AddRightResponse
                {
                    Reportee = orgnr.ToString(),
                    OperationResult = ruleAlreadyExists,
                    Right = RegisterSRRRightsType.Write,
                    ValidTo = existingValidTo
                }
            };

            serviceClient.AddRights(Arg.Any<int>(), validTo).ReturnsForAnyArgs(addRightsResponseList);

            var client = new SrrClient(serviceClient);
            var result = await client.AddRights(orgnr, validTo);
        }

        [TestMethod]
        public async Task AddRights_NewOrg_ReturnsRights()
        {
            const int orgnr = 123456789;
            DateTime validTo = DateTime.Now.AddYears(2);

            var serviceClient = Substitute.For<IServiceClient>();
            var addRightsOkResponseList = GetAddRightsOkResponseList(orgnr, validTo);
            
            serviceClient.AddRights(Arg.Any<int>(), validTo).ReturnsForAnyArgs(addRightsOkResponseList);

            var client = new SrrClient(serviceClient);
            var result = await client.AddRights(orgnr, validTo);
            
            Assert.AreEqual(validTo, result.ReadRightValidTo);
            Assert.AreEqual(validTo, result.WriteRightValidTo);
            Assert.IsTrue(result.HasMoveRights);
        }

        [TestMethod]
        public async Task UpdateRights_ExistingOrg_ReturnsDeleteAndAddCalled()
        {
            const int orgnr = 123456789;
            DateTime validTo = DateTime.Now.AddYears(2);

            var serviceClient = Substitute.For<IServiceClient>();
            var deleteRightsResponseList = GetDeleteRightsOkResponseList(orgnr);
            var addRightsOkResponseList = GetAddRightsOkResponseList(orgnr, validTo);

            serviceClient.DeleteRights(Arg.Any<int>()).ReturnsForAnyArgs(deleteRightsResponseList);
            serviceClient.AddRights(Arg.Any<int>(),validTo).ReturnsForAnyArgs(addRightsOkResponseList);

            var client = new SrrClient(serviceClient);
            var result = await client.UpdateRights(orgnr, validTo);
            await serviceClient.Received().DeleteRights(orgnr);
            await serviceClient.Received().AddRights(orgnr, validTo);
        }

        private static AddRightResponseList GetAddRightsOkResponseList(int orgnr, DateTime validTo)
        {
            var updateRightsResponseList = new AddRightResponseList
            {
                GetAddRightItem(orgnr, OperationResult.Ok, RegisterSRRRightsType.Read, validTo),
                GetAddRightItem(orgnr, OperationResult.Ok, RegisterSRRRightsType.Write, validTo)
            };
            return updateRightsResponseList;
        }

        private static DeleteRightResponseList GetDeleteRightsOkResponseList(int orgnr)
        {
            var deleteRightsResponseList = new DeleteRightResponseList
            {
                GetDeleteRightItem(orgnr, OperationResult.Ok, RegisterSRRRightsType.Read),
                GetDeleteRightItem(orgnr, OperationResult.Ok, RegisterSRRRightsType.Write),
            };
            return deleteRightsResponseList;
        }

        [TestMethod]
        public async Task UpdateRights_NonExistingOrg_OrgIsAdded()
        {
            const int orgnr = 123456789;
            const OperationResult notOkResult = OperationResult.RuleNotFound;
            DateTime validTo = DateTime.Now.AddYears(2);

            var serviceClient = Substitute.For<IServiceClient>();
            var deleteRightsResponseList = new DeleteRightResponseList
            {
                GetDeleteRightItem(orgnr, notOkResult, RegisterSRRRightsType.Read),
                GetDeleteRightItem(orgnr, notOkResult, RegisterSRRRightsType.Write),
            };

            serviceClient.DeleteRights(Arg.Any<int>()).ReturnsForAnyArgs(deleteRightsResponseList);
            serviceClient.AddRights(Arg.Any<int>(),
                Arg.Any<DateTime>()).ReturnsForAnyArgs(GetAddRightsOkResponseList(orgnr, validTo));

            var client = new SrrClient(serviceClient);
            var result = await client.UpdateRights(orgnr, DateTime.Now.AddYears(2));
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

        private static AddRightResponse GetAddRightItem(int orgnr, OperationResult result, RegisterSRRRightsType right, DateTime validTo)
        {
            return new AddRightResponse
            {
                Reportee = orgnr.ToString(),
                OperationResult = result,
                Right = right,
                ValidTo = validTo
                
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
