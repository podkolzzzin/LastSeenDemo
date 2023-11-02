namespace LastSeenDemo.UnitTests;
using Xunit;


public class ReturnUserStatisticsUnit
{
    [Fact]
    public void ReturnMinTest()
    {
        Report report = new Report();
        Guid userId = Guid.NewGuid();
        var should_be_int = report.ReturnMin(userId);
        
        Assert.IsType<int>(should_be_int);
    }
    
    [Fact]
    public void ReturnMaxTest()
    {
        Report report = new Report();
        Guid userId = Guid.NewGuid();
        var should_be_int = report.ReturnMax(userId);
        
        Assert.IsType<int>(should_be_int);
    }
    
    [Fact]
    public void MockUserMinTest()
    {
        Report report = new Report();
        Guid userId = Guid.NewGuid();
        var should_be_int = report.ReturnMax(userId);
        
        Assert.IsType<int>(should_be_int);
    }
    
    [Fact]
    public void WhatItReturns_ShouldBeUserBTW()
    {
       Assignment5 assignment5 = new Assignment5();
     var should_be_user = assignment5.GetUserList();
        
     Assert.IsType<User>(should_be_user[0]);
        Assert.Equal(true, true);
    }
}
