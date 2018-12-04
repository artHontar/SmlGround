using System;
using System.Collections.Generic;
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
        [SetUp]
        public void Init()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            userService = new UserService(_unitOfWork.Object);
        }

        
    }
}
