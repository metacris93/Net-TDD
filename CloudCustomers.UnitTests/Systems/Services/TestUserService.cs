using System;
using CloudCustomers.API.Config;
using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace CloudCustomers.UnitTests.Systems.Services;

public class TestUserService
{
    [Fact]
    public async Task GetAllUSers_WhenCalled_InvokesHttpGetRequest()
    {
        //arrange
        var expected = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expected);
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(
            new UsersApiOptions
            {
                Endpoint = "https://example.com"
            });
        var sut = new UserService(httpClient, config);
        //act
        await sut.GetAllUsers();
        //assert
        handlerMock
            .Protected()
            .Verify(
                "SendAsync"
                , Times.Exactly(1)
                , ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get)
                , ItExpr.IsAny<CancellationToken>()
            );
        //verify HTTP Request is made!
    }
    [Fact]
    public async Task GetAllUsers_WhenHits404_ReturnsEmptyListOfUSers()
    {
        //arrange
        var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(
            new UsersApiOptions
            {
                Endpoint = "https://example.com"
            });
        var sut = new UserService(httpClient, config);
        //act
        var result = await sut.GetAllUsers();
        //assert
        result.Count.Should().Be(0);
    }
    [Fact]
    public async Task GetAllUsers_WhenCalled_ReturnsListOfUsersOfExpectedSize()
    {
        //arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(
            new UsersApiOptions
            {
                Endpoint = "https://example.com"
            });
        var sut = new UserService(httpClient, config);
        //act
        var result = await sut.GetAllUsers();
        //assert
        result.Count.Should().Be(expectedResponse.Count);
    }
    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
    {
        //arrange
        var endpoint = "https://example.com/users";
        var expectedResponse = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>
            .SetupBasicGetResourceList(expectedResponse, endpoint);
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(
            new UsersApiOptions {
                Endpoint = endpoint
            }
        );
        var sut = new UserService(httpClient, config);
        //act
        var result = await sut.GetAllUsers();
        var uri = new Uri(endpoint);
        //assert
        handlerMock
            .Protected()
            .Verify(
                "SendAsync"
                , Times.Exactly(1)
                , ItExpr.Is<HttpRequestMessage>(
                    req => req.Method == HttpMethod.Get && req.RequestUri == uri)
                , ItExpr.IsAny<CancellationToken>()
            );
    }
}
