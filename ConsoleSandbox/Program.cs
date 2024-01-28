using Dynastream.Fit;
using System.Buffers.Text;

const ushort ProductId = 0; // ???

GenerateEveryPossibleExercise.Generate();
return;

///
/// Below is old experiments, from when I was banging against Garmin's website
/// till it finally accepted something.
///

// 1. Create the output stream, this can be any type of stream, including a file or memory stream. Must have read/write access.
var outStream = new FileStream("out.fit", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

// 2. Create a FIT Encode object.
Encode encoder = new Encode(ProtocolVersion.V20);

// 3. Write the FIT header to the output stream.
encoder.Open(outStream);

// The timestamp for the workout file
uint duration = 1000;
var timeCreated = new Dynastream.Fit.DateTime(System.DateTime.UtcNow);
var timeStarted = new Dynastream.Fit.DateTime(timeCreated);
var timeStopped = new Dynastream.Fit.DateTime(timeCreated.GetTimeStamp() + duration);


// 4. Every FIT file MUST contain a File ID message as the first message
var fileIdMesg = new FileIdMesg();
fileIdMesg.SetType(Dynastream.Fit.File.Activity);
fileIdMesg.SetManufacturer(Manufacturer.Development);
fileIdMesg.SetProduct(ProductId);
fileIdMesg.SetSerialNumber(timeCreated.GetTimeStamp());
fileIdMesg.SetTimeCreated(timeCreated);
encoder.Write(fileIdMesg);

//byte[] appId = {
//    0x3, 0x2, 0x1, 0x0,
//    0x5, 0x4, 0x7, 0x6,
//    0x8, 0x9, 0x10, 0x11,
//    0x12, 0x13, 0x14, 0x15
//};
//var developerIdMesg = new DeveloperDataIdMesg();
//for (int i = 0; i < appId.Length; i++)
//{
//    developerIdMesg.SetApplicationId(i, appId[i]);
//}
//developerIdMesg.SetDeveloperDataIndex(0);
//developerIdMesg.SetApplicationVersion(110);
//encoder.Write(developerIdMesg);

//var lDeviceInfo = new DeviceInfoMesg();
//lDeviceInfo.SetTimestamp(timeStarted);
//lDeviceInfo.SetDeviceIndex(0);
//lDeviceInfo.SetManufacturer(Manufacturer.Development);
//lDeviceInfo.SetProduct(0);
//lDeviceInfo.SetProductName("Test");
//lDeviceInfo.SetSerialNumber(12345);
//lDeviceInfo.SetSoftwareVersion(1.0f);
//encoder.Write(lDeviceInfo);

//var lEvent = new EventMesg();
//lEvent.SetTimestamp(timeStarted);
//lEvent.SetEvent(Event.Timer /*0*/);
//lEvent.SetEventType(EventType.Start/* 0*/);
//encoder.Write(lEvent);

//var sportMesg = new SportMesg();
//sportMesg.SetName("Run");
//sportMesg.SetSport(Sport.Running);
//sportMesg.SetSubSport(SubSport.Generic);
//encoder.Write(sportMesg);

//var lEvent2 = new EventMesg();
//lEvent2.SetTimestamp(timeStopped);
//lEvent2.SetEvent(Event.Timer/*0*/);
//lEvent2.SetEventType(EventType.StopAll/* 4*/);
//encoder.Write(lEvent2);

//lDeviceInfo = new DeviceInfoMesg();
//lDeviceInfo.SetTimestamp(timeStopped);
//lDeviceInfo.SetDeviceIndex(0);
//lDeviceInfo.SetManufacturer(Manufacturer.Development);
//lDeviceInfo.SetProduct(0);
//lDeviceInfo.SetProductName("Test");
//lDeviceInfo.SetSerialNumber(12345);
//lDeviceInfo.SetSoftwareVersion(1.0f);
//encoder.Write(lDeviceInfo);

ushort setCount = 0;
for (ushort category = 0; category < 33; category++) {
    for (ushort subtype = 0; subtype < 1; subtype++) {
        var set = new SetMesg();
        //set.SetTimestamp(timeStarted);
        //set.SetMessageIndex(setCount++);
        set.SetStartTime(timeStarted);
        set.SetDuration(0);
        //set.SetCategory(0, category);
        //set.SetCategorySubtype(0, subtype);
        //set.SetRepetitions(0);
        set.SetSetType(SetType.Active);
        encoder.Write(set);
    }
}

var sessionMesg = new SessionMesg();
sessionMesg.SetTimestamp(timeStopped);
sessionMesg.SetStartTime(timeStarted);
sessionMesg.SetTotalElapsedTime(1); //Total number of msec since timer started (includes pauses) - Todo: calculate paused time
sessionMesg.SetTotalTimerTime(10); //Timer Time (excludes pauses)
sessionMesg.SetSport(Sport.Training);
sessionMesg.SetSubSport(SubSport.CardioTraining);
sessionMesg.SetNumLaps(setCount);
sessionMesg.SetEvent(Event.Lap);
sessionMesg.SetEventType(EventType.Stop);
encoder.Write(sessionMesg);

// Adding this made it work!!!
var activityMesg = new ActivityMesg();
activityMesg.SetTimestamp(timeStopped);
activityMesg.SetTotalTimerTime(1);
activityMesg.SetNumSessions(1);
activityMesg.SetType(Activity.Manual);
activityMesg.SetEvent(Event.Activity);
activityMesg.SetEventType(EventType.Stop);
encoder.Write(activityMesg);

// 5. Every FIT Workout file MUST contain a Workout message as the second message
//var activityMesg = new ActivityMesg();
//encoder.Write(activityMesg);

//ushort stepIndex = 0;
//// 6. Every FIT Workout file MUST contain one or more Workout Step messages
//var workoutStepMesg = new WorkoutStepMesg();
//workoutStepMesg.SetMessageIndex(stepIndex++);
//workoutStepMesg.SetExerciseCategory(ExerciseCategory.Unknown);
//workoutStepMesg.SetExerciseName(1001);
////workoutStepMesg.SetWktStepName("Endurance Ride");
//workoutStepMesg.SetNotes("keep upper leg vertical, avoid pelvic tilt");
////workoutStepMesg.SetIntensity(Intensity.Invalid);
//workoutStepMesg.SetDurationType(WktStepDuration.Time);
//workoutStepMesg.SetDurationValue(36000000); // milliseconds
//workoutStepMesg.SetTargetType(WktStepTarget.HeartRate);
//workoutStepMesg.SetTargetValue(2);
//encoder.Write(workoutStepMesg);

//workoutStepMesg = new WorkoutStepMesg();
//workoutStepMesg.SetMessageIndex(stepIndex++);
//workoutStepMesg.SetExerciseCategory(ExerciseCategory.WarmUp);
//workoutStepMesg.SetExerciseName(1000);
////workoutStepMesg.SetWktStepName("Endurance Ride");
//workoutStepMesg.SetNotes("keep upper leg vertical, avoid pelvic tilt");
////workoutStepMesg.SetIntensity(Intensity.Invalid);
//workoutStepMesg.SetDurationType(WktStepDuration.Time);
//workoutStepMesg.SetDurationValue(36000000); // milliseconds
//workoutStepMesg.SetTargetType(WktStepTarget.HeartRate);
//workoutStepMesg.SetTargetValue(2);
//encoder.Write(workoutStepMesg);

//ushort titleIndex = 0;
//var exerciseTitleMessage = new ExerciseTitleMesg();
//exerciseTitleMessage.SetMessageIndex(titleIndex++);
//exerciseTitleMessage.SetWktStepName(0, "exercise title 1");
//exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.WarmUp);
//exerciseTitleMessage.SetExerciseName(1001);
//encoder.Write(exerciseTitleMessage);

//exerciseTitleMessage = new ExerciseTitleMesg();
//exerciseTitleMessage.SetMessageIndex(titleIndex++);
//exerciseTitleMessage.SetWktStepName(0, "exercise title 2");
//exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.WarmUp);
//exerciseTitleMessage.SetExerciseName(1000);
//encoder.Write(exerciseTitleMessage);

//exerciseTitleMessage = new ExerciseTitleMesg();
//exerciseTitleMessage.SetMessageIndex(titleIndex++);
//exerciseTitleMessage.SetWktStepName(0, "exercise title 3");
//exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.Unknown);
//exerciseTitleMessage.SetExerciseName(1001);
//encoder.Write(exerciseTitleMessage);

//exerciseTitleMessage = new ExerciseTitleMesg();
//exerciseTitleMessage.SetMessageIndex(titleIndex++);
//exerciseTitleMessage.SetWktStepName(0, "exercise title 4");
//exerciseTitleMessage.SetExerciseCategory(ExerciseCategory.Unknown);
//exerciseTitleMessage.SetExerciseName(1000);
//encoder.Write(exerciseTitleMessage);


// 7. Update the data size in the header and calculate the CRC
encoder.Close();

// 8. Close the output stream
outStream.Close();

outStream.Dispose();