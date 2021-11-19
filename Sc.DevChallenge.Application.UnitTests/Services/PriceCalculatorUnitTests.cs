﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sc.DevChallenge.Application.Common;
using Sc.DevChallenge.Application.Common.Exceptions.ApiException;
using Sc.DevChallenge.Application.Services;
using Sc.DevChallenge.Domain.Entities;
using Sc.DevChallenge.Domain.ValueObjects;
using Xunit;

namespace Sc.DevChallenge.Application.UnitTests.Services
{
    public class PriceCalculatorUnitTests
    {
        private readonly ApplicationSettings _applicationSettings = new()
        {
            GeneralStartPoint = new DateTime(2018, 1, 1),
            TimeIntervalInSec = 10000
        };

        [Theory]
        [MemberData(nameof(GetAveragePriceTestData))]
        public void CalculateAveragePrice_ShouldReturnProperValue(List<PriceEntity> data, decimal expectedResult)
        {
            // Arrange
            var priceCalculator = new PriceCalculator(_applicationSettings);

            // Act 
            var result = priceCalculator.CalculateAveragePrice(data);
            
            // Assert
            result.Should().Be(expectedResult);
        }


        [Theory]
        [MemberData(nameof(GetTimeSlotTestData))]
        public void CalculatePriceTimeSlot_ShouldReturnProperTimeSlot(DateTime data, PriceTimeSlot expectedValue)
        {
            // Arrange
            var priceCalculator = new PriceCalculator(_applicationSettings);

            // Act
            var result = priceCalculator.CalculatePriceTimeSlot(data);

            // Assert
            result.Should().Be(expectedValue);

        }

        [Fact]
        public void CalculatePriceTimeInterval_ShouldThrowAnException_WhenDataLowerThanStartPointPassed()
        {
            // Arrange
            var priceCalculator = new PriceCalculator(_applicationSettings);
            
            // Act
            Action action = () => priceCalculator.CalculatePriceTimeSlot(new DateTime(2015, 1, 1));
            
            //Assert
            action.Should().Throw<BadRequestException>()
                .WithMessage("Passed datetime is less than the start point");

        }


        [Fact]
        public void CalculatePriceTimeInterval_ShouldNotThrowAnException_WhenDataBiggerThanStartPointPassed()
        {
            // Arrange
            var priceCalculator = new PriceCalculator(_applicationSettings);
            
            // Act
            Action action = () => priceCalculator.CalculatePriceTimeSlot(new DateTime(2019, 1, 1));
            
            //Assert
            action.Should().NotThrow<BadRequestException>();
        }


        #region Test data
        public static IEnumerable<object[]> GetAveragePriceTestData()
        {
            yield return new object[]
            {
                new List<PriceEntity>
                {
                    new() {Price = new decimal(124.164)},
                    new() {Price = new decimal(10.26)},
                    new() {Price = new decimal(19.24)},
                    new() {Price = new decimal(179.16)},
                },
                new decimal(83.21)
            }; 

            yield return new object[]
            {
                new List<PriceEntity>
                {
                    new() {Price = new decimal(789.606)},
                    new() {Price = new decimal(420.69)},
                    new() {Price = new decimal(69.420)},
                    new() {Price = new decimal(354.167)},
                    new() {Price = new decimal(10.759)},
                    new() {Price = new decimal(5.13)},
                },
                new decimal(274.96)
            };

            yield return new object[]
            {
                new List<PriceEntity>()
                {
                    new() {Price = new decimal(148.10)},
                    new() {Price = new decimal(1097.26)},
                    new() {Price = new decimal(20197.16)},
                    new() {Price = new decimal(12456.21)},
                    new() {Price = new decimal(1423.09)},
                    new() {Price = new decimal(784.93)},
                    new() {Price = new decimal(420.21)},
                    new() {Price = new decimal(659.14)},
                    new() {Price = new decimal(120.167)},
                    new() {Price = new decimal(157.9874)},
                    new() {Price = new decimal(1791.20)},
                    new() {Price = new decimal(341.197)},
                },
                new decimal(3299.72)
            };
        }

        public static IEnumerable<object[]> GetTimeSlotTestData()
        {
            yield return new object[]
            {
                new DateTime(2018, 1, 1),
                PriceTimeSlot.From((new(2018, 1, 1), new(2018, 1, 1, 2, 46, 40)))
            };
            
            yield return new object[]
            {
                new DateTime(2019, 05, 14, 11,26,13),
                PriceTimeSlot.From((
                    new(2019, 5, 14, 10,18,26),
                    new(2019, 5, 14, 13, 05, 06)))
            };
            
            yield return new object[]
            {
                new DateTime(2021, 11, 05, 02,12,10),
                PriceTimeSlot.From((
                    new(2021, 11, 5, 01,48,50),
                    new(2021, 11, 5, 04, 35, 30)))
            };
            
            yield return new object[]
            {
                new DateTime(2024, 01, 13, 23,58,16),
                PriceTimeSlot.From((
                    new(2024, 01, 13, 22,10,40),
                    new(2024, 01, 14, 00, 57, 20)))
            };
            
            
        }
        #endregion
        
    }
}