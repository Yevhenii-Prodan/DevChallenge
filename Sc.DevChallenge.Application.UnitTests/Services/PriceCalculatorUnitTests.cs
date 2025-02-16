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

        [Theory]
        [MemberData(nameof(GetQuartileTestData))]
        public void CalculateQuartile_ShodReturnProperValue(int n, int quartile, int expectedResult)
        {
            
            // Arrange
            var priceCalculator = new PriceCalculator(_applicationSettings);
            
            // Act 
            var result = priceCalculator.CalculateQuartile(n, quartile);
            
            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void CalculateQuartile_ShouldThrowException_WhenIncorrectDataPassed()
        {
            // Arrange   
            var priceCalculator = new PriceCalculator(_applicationSettings);

            // Act
            Action act1 = () => priceCalculator.CalculateQuartile(10, 0);
            Action act2 = () => priceCalculator.CalculateQuartile(10, 5);
            
            //Assert
            act1.Should().Throw<ArgumentException>();
            act2.Should().Throw<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(GetBenchmarkPriceTestData))]
        public void CalculateBenchmarkPrice_ShouldReturnProperValue(List<PriceEntity> prices, decimal expectedResult)
        {
            // Arrange   
            var priceCalculator = new PriceCalculator(_applicationSettings);
            
            // Act
            var result = priceCalculator.CalculateBenchmarkPrice(prices);
            
            // Assert
            result.Should().Be(expectedResult);
        }


        #region Test data

        public static IEnumerable<object[]> GetBenchmarkPriceTestData()
        {
            yield return new object[]
            {
                new List<PriceEntity>()
                {
                    new () {Price = new decimal(174.5)},
                    new () {Price = new decimal(25.6)},
                    new () {Price = new decimal(59.2)},
                    new () {Price = new decimal(113.54)},
                    new () {Price = new decimal(11.6)},
                    new () {Price = new decimal(3.14)},
                    new () {Price = new decimal(69.420)},
                    new () {Price = new decimal(123.16)},
                },
                72.52
            };

            var list = new List<PriceEntity>()
            {
                new() {Price = new decimal(174.5)},
                new() {Price = new decimal(25.6)},
                new() {Price = new decimal(59.2)},
            };

            yield return new object[]
            {
                list, Math.Round(list.Average(x => x.Price), 2)
            };
        }

        public static IEnumerable<object[]> GetQuartileTestData()
        {
            yield return new object[]
            {
                8, 1, 2
            };


            yield return new object[]
            {
                10,3,7
            };

            yield return new object[]
            {
                25,3,18
            };

            yield return new object[]
            {
                12,1,3
            };

            yield return new object[]
            {
                150,3,112
            };
        }

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
                PriceTimeSlot.From((new(2018, 1, 1), new(2018, 1, 1, 2, 46, 39)))
            };
            
            yield return new object[]
            {
                new DateTime(2019, 05, 14, 11,26,13),
                PriceTimeSlot.From((
                    new(2019, 5, 14, 09,06,40),
                    new(2019, 5, 14, 11, 53, 19)))
            };
            
            yield return new object[]
            {
                new DateTime(2021, 11, 05, 02,12,10),
                PriceTimeSlot.From((
                    new(2021, 11, 5, 01,13,20),
                    new(2021, 11, 5, 03, 59, 59)))
            };
            
            yield return new object[]
            {
                new DateTime(2024, 01, 13, 23,58,16),
                PriceTimeSlot.From((
                    new(2024, 01, 13, 22,26,40),
                    new(2024, 01, 14, 01, 13, 19)))
            };
            
            
        }
        #endregion
        
    }
}