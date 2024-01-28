using Dynastream.Fit;

namespace GarminWorkoutBuilder
{
    public static class Functions
    {
        public static void Generate(out string message, out string base64) {
            const ushort ProductId = 1; // ???

            // 1. Create the output stream, this can be any type of stream, including a file or memory stream. Must have read/write access.
            var outStream = new MemoryStream();

            // 2. Create a FIT Encode object.
            Encode encoder = new Encode(ProtocolVersion.V10);

            // 3. Write the FIT header to the output stream.
            encoder.Open(outStream);

            // The timestamp for the workout file
            var timeCreated = new Dynastream.Fit.DateTime(System.DateTime.UtcNow);

            // 4. Every FIT file MUST contain a File ID message as the first message
            var fileIdMesg = new FileIdMesg();
            fileIdMesg.SetType(Dynastream.Fit.File.Workout);
            fileIdMesg.SetManufacturer(Manufacturer.Development);
            fileIdMesg.SetProduct(ProductId);
            fileIdMesg.SetSerialNumber(timeCreated.GetTimeStamp());
            fileIdMesg.SetTimeCreated(timeCreated);
            encoder.Write(fileIdMesg);

            // 5. Every FIT Workout file MUST contain a Workout message as the second message
            var workoutMesg = new WorkoutMesg();
            workoutMesg.SetWktName("cardio-type3 workout");
            workoutMesg.SetSport(Sport.Training);
            workoutMesg.SetSubSport(SubSport.CardioTraining);
            workoutMesg.SetNumValidSteps(2);
            encoder.Write(workoutMesg);

            ushort stepIndex = 0;
            // 6. Every FIT Workout file MUST contain one or more Workout Step messages
            var workoutStepMesg = new WorkoutStepMesg();
            workoutStepMesg.SetMessageIndex(stepIndex++);
            workoutStepMesg.SetExerciseCategory(ExerciseCategory.Unknown);
            workoutStepMesg.SetExerciseName(1001);
            //workoutStepMesg.SetWktStepName("Endurance Ride");
            workoutStepMesg.SetNotes("keep upper leg vertical, avoid pelvic tilt");
            //workoutStepMesg.SetIntensity(Intensity.Invalid);
            workoutStepMesg.SetDurationType(WktStepDuration.Time);
            workoutStepMesg.SetDurationValue(36000000); // milliseconds
            workoutStepMesg.SetTargetType(WktStepTarget.HeartRate);
            workoutStepMesg.SetTargetValue(2);
            encoder.Write(workoutStepMesg);

            workoutStepMesg = new WorkoutStepMesg();
            workoutStepMesg.SetMessageIndex(stepIndex++);
            workoutStepMesg.SetExerciseCategory(ExerciseCategory.WarmUp);
            workoutStepMesg.SetExerciseName(1000);
            //workoutStepMesg.SetWktStepName("Endurance Ride");
            workoutStepMesg.SetNotes("keep upper leg vertical, avoid pelvic tilt");
            //workoutStepMesg.SetIntensity(Intensity.Invalid);
            workoutStepMesg.SetDurationType(WktStepDuration.Time);
            workoutStepMesg.SetDurationValue(36000000); // milliseconds
            workoutStepMesg.SetTargetType(WktStepTarget.HeartRate);
            workoutStepMesg.SetTargetValue(2);
            encoder.Write(workoutStepMesg);

            ushort titleIndex = 0;
            var exerciseTitleMessage = new ExerciseTitleMesg();
            exerciseTitleMessage.SetMessageIndex(titleIndex++);
            exerciseTitleMessage.SetWktStepName(0, "exercise title 1");
            exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.WarmUp);
            exerciseTitleMessage.SetExerciseName(1001);
            encoder.Write(exerciseTitleMessage);
            
            exerciseTitleMessage = new ExerciseTitleMesg();
            exerciseTitleMessage.SetMessageIndex(titleIndex++);
            exerciseTitleMessage.SetWktStepName(0, "exercise title 2");
            exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.WarmUp);
            exerciseTitleMessage.SetExerciseName(1000);
            encoder.Write(exerciseTitleMessage);

            exerciseTitleMessage = new ExerciseTitleMesg();
            exerciseTitleMessage.SetMessageIndex(titleIndex++);
            exerciseTitleMessage.SetWktStepName(0, "exercise title 3");
            exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.Unknown);
            exerciseTitleMessage.SetExerciseName(1001);
            encoder.Write(exerciseTitleMessage);

            exerciseTitleMessage = new ExerciseTitleMesg();
            exerciseTitleMessage.SetMessageIndex(titleIndex++);
            exerciseTitleMessage.SetWktStepName(0, "exercise title 4");
            exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.Unknown);
            exerciseTitleMessage.SetExerciseName(1000);
            encoder.Write(exerciseTitleMessage);

            // 7. Update the data size in the header and calculate the CRC
            encoder.Close();

            // 8. Close the output stream
            outStream.Close();

            base64 = Convert.ToBase64String(outStream.ToArray());
            outStream.Dispose();

            message = workoutMesg.GetWktNameAsString();
            for (byte i = 0; i < workoutStepMesg.GetNumFields(); i++)
            {
                var field = workoutStepMesg.GetField(i);
                if (field != null)
                {
                    var value = field.GetValue();
                    message += "\n" + field.GetName() + ":";
                    if(value is byte[] b)
                    {
                        message += System.Text.Encoding.ASCII.GetString(b);
                    }
                    else
                    {
                        message += value;
                    }
                }
                
            }
        }
    }
}
