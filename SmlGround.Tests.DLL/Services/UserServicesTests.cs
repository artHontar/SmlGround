using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Common.Enum;
using Moq;
using NUnit.Framework;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Interfaces;
using SmlGround.DLL.Service;
using Profile = SmlGround.DataAccess.Models.Profile;

namespace SmlGround.Tests.DLL.Services
{
    [TestFixture]
    public class UserServicesTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private UserService userService;
        [OneTimeSetUp]
        public void InitOnce()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Profile, ProfileDTO>();
            });
            _unitOfWork = new Mock<IUnitOfWork>();
            userService = new UserService(_unitOfWork.Object);
        }
        [SetUp]
        public void Init()
        {
            //_unitOfWork = new Mock<IUnitOfWork>();
            //userService = new UserService(_unitOfWork.Object);
        }
        [Test]
        public async Task TestGetAllProfiles_SearchNull_IsNotNull()
        {
            // Arrange
            Profile currentUser = new Profile { Id = "1" };
            var list = new List<Profile> {currentUser, new Profile{ Id = "2" },
                new Profile{ Id = "3" }, new Profile{ Id = "4" } };
            var list1 = new List<Friend> { new Friend { UserById = "1", UserToId = "2", CreationTime = DateTime.Now, FriendRequestFlag = FriendStatus.None } };

            _unitOfWork.Setup(c => c.ProfileManager.GetAllAsync()).ReturnsAsync(list);
            _unitOfWork.Setup(c => c.ProfileManager.GetAsync("1")).ReturnsAsync(currentUser);
            _unitOfWork.Setup(c => c.FriendManager.GetAllAsync()).ReturnsAsync(list1);
            
            
            // Act
            var result = await userService.GetAllProfilesAsync("1", null);
            
            // Assert
            CollectionAssert.IsNotEmpty(result);
        }
        
        [Test]
        public async Task TestGetAllProfiles_SearchNotNull_IsNotNull()
        {
            // Arrange
            Profile currentUser = new Profile { Id = "1" };
            IEnumerable<Profile> list = new List<Profile> { new Profile { Id = "1", Name = "Игарь", Surname = "vxc" }, new Profile { Id = "2", Surname = "Игарь" , Name = "ger" }, new Profile { Id = "3", Surname = "Васькин" , Name = "REe" }, new Profile{Id = "4", Name = "wef",Surname = "xcvb"} };
            var list1 = new List<Friend> { new Friend { UserById = "1", UserToId = "2", CreationTime = DateTime.Now, FriendRequestFlag = FriendStatus.None } };

            _unitOfWork.Setup(c => c.ProfileManager.GetAllAsync()).ReturnsAsync(list);
            _unitOfWork.Setup(c => c.ProfileManager.GetAsync("1")).ReturnsAsync(currentUser);
            _unitOfWork.Setup(c => c.FriendManager.GetAllAsync()).ReturnsAsync(list1);

            
            // Act
            var result = await userService.GetAllProfilesAsync("1", "И");

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public async Task TestGetAllProfiles_SearchNotNull_AreEqual_1()
        {
            // Arrange
            Profile currentUser = new Profile { Id = "1" };
            IEnumerable<Profile> list = new List<Profile> { new Profile { Id = "1", Name = "Игарь", Surname = "vxc" }, new Profile { Id = "2", Surname = "Игарь", Name = "ger" }, new Profile { Id = "3", Surname = "Васькин", Name = "REe" }, new Profile { Id = "4", Name = "wef", Surname = "xcvb" } };
            var list1 = new List<Friend> { new Friend { UserById = "1", UserToId = "2", CreationTime = DateTime.Now, FriendRequestFlag = FriendStatus.None } };

            _unitOfWork.Setup(c => c.ProfileManager.GetAllAsync()).ReturnsAsync(list);
            _unitOfWork.Setup(c => c.ProfileManager.GetAsync("1")).ReturnsAsync(currentUser);
            _unitOfWork.Setup(c => c.FriendManager.GetAllAsync()).ReturnsAsync(list1);


            // Act
            var result = await userService.GetAllProfilesAsync("1", "И");

            // Assert
            Assert.AreEqual(1,result.Count());
        }

        [Test]
        //[ExpectedException(typeof(ArgumentException))]
        public async Task TestGetAllProfiles_SearchNotNull_Fail()
        {
            // Arrange
            Profile currentUser = new Profile { Id = "1" };
            IEnumerable<Profile> list = new List<Profile> { new Profile { Id = "1", Surname = "vxc" }, new Profile { Id = "2", Surname = "Игарь", Name = "ger" }, new Profile { Id = "3", Surname = "Васькин", Name = "REe" }, new Profile { Id = "4", Name = "wef", Surname = "xcvb" } };
            var list1 = new List<Friend> { new Friend { UserById = "1", UserToId = "2", CreationTime = DateTime.Now, FriendRequestFlag = FriendStatus.None } };

            _unitOfWork.Setup(c => c.ProfileManager.GetAllAsync()).ReturnsAsync(list);
            _unitOfWork.Setup(c => c.ProfileManager.GetAsync("1")).ReturnsAsync(currentUser);
            _unitOfWork.Setup(c => c.FriendManager.GetAllAsync()).ReturnsAsync(list1);


            // Act
            try
            {
                var result = await userService.GetAllProfilesAsync("1", "И");
                
            }
            catch (NullReferenceException)
            {
                Assert.Fail();
            }
            
            // Assert
        }
    }
}
