using Dynastream.Fit;
using System.Buffers.Text;
public class GenerateEveryPossibleExercise {
    // At time of writing:
    // 54 known categories, 0 to 53
    // Including category = 54 lets us verify that garmin does not recognize it, meaning 53 is the last one
    const int maxCategories = 55;

    // In most categories, once the subtype is too high and garmin doesn't recognize it, the exercise gets the generic name of the category
    // In some categories, there's a block of "Choose an exercise" (without a clickable link to do so) before the generic name
    const int maxSubTypesInAnyCategory = 200;
    
    public static void Generate() {
        const ushort ProductId = 0;
        var outStream = new FileStream("out.fit", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        Encode encoder = new Encode(ProtocolVersion.V20);
        encoder.Open(outStream);

        // Timestamps
        uint duration = 1000;
        var timeCreated = new Dynastream.Fit.DateTime(System.DateTime.UtcNow);
        var timeStarted = new Dynastream.Fit.DateTime(timeCreated);
        var timeStopped = new Dynastream.Fit.DateTime(timeCreated.GetTimeStamp() + duration);

        // FileID
        var fileIdMesg = new FileIdMesg();
        fileIdMesg.SetType(Dynastream.Fit.File.Activity);
        fileIdMesg.SetManufacturer(Manufacturer.Development);
        fileIdMesg.SetProduct(ProductId);
        fileIdMesg.SetSerialNumber(timeCreated.GetTimeStamp());
        fileIdMesg.SetTimeCreated(timeCreated);
        encoder.Write(fileIdMesg);

        // Generate a set for every possible exercise category and subtype
        ushort setCount = 0;
        // for (ushort category = 0; category <= lastCategory; category++) {
        for (ushort category = 0; category < maxCategories; category++) {
            for (ushort subtype = 0; subtype < maxSubTypesInAnyCategory; subtype++) {
                var set = new SetMesg();

                set.SetTimestamp(timeStarted);
                set.SetMessageIndex(setCount++);

                // Garmin refuses to upload without StartTime, Duration, and SetType.  All other fields are apparently
                // optional.
                set.SetStartTime(timeStarted);
                set.SetDuration(0);
                set.SetSetType(SetType.Active);

                set.SetCategory(0, category);
                set.SetCategorySubtype(0, subtype);
                
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

        encoder.Close();
        outStream.Close();
        outStream.Dispose();
    }
}