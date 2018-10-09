using System;
using System.Collections.Generic;

namespace SEOAnalyser.UnitTests
{
    public class OccurrenceModelComparer : IEqualityComparer<OccurrenceModel>
    {
        public bool Equals(OccurrenceModel x, OccurrenceModel y)
        {
            //Check whether the objects are the same object. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether the OccurrenceModel's properties are equal. 
            return x != null && y != null && x.OccurrenceWordOrLink.Equals(y.OccurrenceWordOrLink) && x.OccurrenceCount.Equals(y.OccurrenceCount);
        }

        public int GetHashCode(OccurrenceModel obj)
        {
            //Get hash code for the OccurrenceWordOrLink field if it is not null. 
            int hashOccurrenceWordOrLink = obj.OccurrenceWordOrLink == null ? 0 : obj.OccurrenceWordOrLink.GetHashCode();

            //Get hash code for the OccurrenceCount field. 
            int hashOccurrenceCount = obj.OccurrenceCount.GetHashCode();

            //Calculate the hash code for the OccurrenceModel. 
            return hashOccurrenceWordOrLink ^ hashOccurrenceCount;
        }
    }
}
