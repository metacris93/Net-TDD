using System;
using CloudCustomers.API.Controllers;
using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using CloudCustomers.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CloudCustomers.UnitTests.Systems.Controllers;

public class TestUsersController
{
    [Fact]
    public async Task Get_OnSuccess_ReturnStatusCode200()
    {
        //arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());
        var sut = new UsersController(mockUserService.Object);
        //act
        var result = (OkObjectResult)await sut.Get();
        //assert
        result.StatusCode.Should().Be(200);
    }
    [Fact]
    public async Task Get_OnSuccess_InvokesUsersServiceExactlyOnce()
    {
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        //arrange
        var sut = new UsersController(mockUserService.Object);
        //act
        var result = await sut.Get();
        //assert
        mockUserService.Verify(service => service.GetAllUsers(), Times.Once());
    }
    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUSers()
    {
        //arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());
        var sut = new UsersController(mockUserService.Object);
        //act
        var result = await sut.Get();
        //assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }
    [Fact]
    public async Task Get_OnNoUsersFound_Returns404()
    {
        //arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        var sut = new UsersController(mockUserService.Object);
        //act
        var result = await sut.Get();
        //assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }
}
