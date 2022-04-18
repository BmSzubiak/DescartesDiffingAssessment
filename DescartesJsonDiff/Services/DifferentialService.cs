using DescartesJsonDiff.Models;
using DescartesJsonDiff.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Services
{
    /// <summary>
    /// Service that handles the creation and checking of differntial json objects
    /// </summary>
    public class DifferentialService : IDifferentialService
    {
        /// <summary>
        /// Method to input the data into an in memory cache.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <param name="side"></param>
        /// <returns>void</returns>
        public void CreateJsonData(string id, ApiInput input, string side)
        {
            //Create an unique id when adding to the in memory cache so that it is retrievable
            var entryId = id + "-" + side;

            InMemoryCache.AddToCache(entryId, input.data);
        }

        /// <summary>
        /// Method to check the differential between the two inputs (left and right)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResponse</returns>
        public ApiResponse GetJsonDiff(string id)
        {
            //Get both sides from the in memory cache
            var leftResult = InMemoryCache.GetFromCache(id + "-" + "left");
            var rightResult = InMemoryCache.GetFromCache(id + "-" + "right");

            if (leftResult == null || rightResult == null)
            {
                return null;
            }

            //Verify if they are the same length, if not return DiffResultType.SizeDoNotMatch
            if(leftResult.Length != rightResult.Length)
            {
                return new ApiResponse()
                {
                    DiffResultType = DiffResultType.SizeDoNotMatch
                };
            }

            //If the data is equal, return DiffResultType.Equals
            if (leftResult.SequenceEqual(rightResult))
            {
                return new ApiResponse()
                {
                    DiffResultType = DiffResultType.Equals
                };
            }


            var diffList = new List<Differential>();
            var lastErrorFound = 0;

            //If the length is the same but the data is not, compare and build the list of differentials
            for (int i = 0; i < leftResult.Length; i++)
            {
                if (!leftResult[i].Equals(rightResult[i]))
                {
                    //Add to differential list
                    diffList.Add(new Differential()
                    {
                        Offset = i,
                        Length = i == 0 ? 1 : i - lastErrorFound
                    });

                    lastErrorFound = i;
                }
            }

            return new ApiResponse() 
            {
                DiffResultType = DiffResultType.ContentDoNotMatch,
                Diffs = diffList
            };
        }
    }
}
