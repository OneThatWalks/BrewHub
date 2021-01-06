using System;

namespace BrewHub.Common.Domain
{
    public class Brew
    {
        public string Title { get; set; }

        public DateTime StartDateTimeUtc { get; set; }

        public DateTime EndDateTimeUtc { get; set; }

        public Ingredient[] Ingredients { get; set; }

        public string[] Supplies { get; set; }

        public string BrewNotes { get; set; }

        public float EstimatedABV { get; set; }

        public Measurement[] GravityReadings { get; set; }

        public Measurement FinalGravity { get; set; }

        public Measurement StartingGravity { get; set; }

        public Recipe Recipe { get; set; }

    }

    public class Ingredient
    {
        public string Text { get; set; }
    }

    public abstract class Measurement
    {
        public DateTime TakenDateTimeUtc { get; set; }

        public float Value { get; set; }

        public string Unit { get; set; }
    }

    public static class SugarMeasurement
    {
        public static Type[] ValidMeasurements { get; set; } = new [] {typeof(SpecificGravityMeasurement), typeof(BrixMeasurement)};

        public static Func<float, double> BrixToSpecificGravityFormula = (float value) => (value / (258.6 - ((value / 258.2) * 227.1))) + 1;
        public static Func<float, double> SpecificGravityToBrixFormula = (float value) => (((182.4601 * value - 775.6821) * value + 1262.7794) * value - 669.5622);

        public static SpecificGravityMeasurement ToSpecificGravityMeasurement(this BrixMeasurement brixMeasurement)
        {
            var brixToSG = BrixToSpecificGravityFormula(brixMeasurement.Value);
            var convertedToSingle = Convert.ToSingle(brixMeasurement);
            return new SpecificGravityMeasurement(convertedToSingle) {TakenDateTimeUtc = brixMeasurement.TakenDateTimeUtc};
        }


        public static BrixMeasurement ToBrixMeasurement(this SpecificGravityMeasurement specificGravityMeasurement)
        {
            var SGtoBrixG = SpecificGravityToBrixFormula(specificGravityMeasurement.Value);
            var convertedToSingle = Convert.ToSingle(SGtoBrixG);
            return new BrixMeasurement(convertedToSingle) { TakenDateTimeUtc = specificGravityMeasurement.TakenDateTimeUtc };
        }
    }

    public class SpecificGravityMeasurement : Measurement
    {
        public SpecificGravityMeasurement(float value) : base()
        {
            // TODO business check
            Value = value;
            Unit = "SG";
        }
    }

    public class BrixMeasurement : Measurement
    {
        public BrixMeasurement(float value) : base()
        {
            // TODO business check
            Value = value;
            Unit = "°Bx";
        }
    }

    public class Recipe
    {

    }
}