## Encoding Activity File

Garmin looks at SessionMesg sport for the sport  
ignores SportMesg

Bare minimum:  
fileId  
session  
activity

Optional:  
fileId  
Event start  
sport  
event stop  
session  
activity

Activity can declare sets without embedded workout

If declare exercise category w/out category subtype, it shows as name of the category (for example: "bench press")

Bare minimum for a SetMesg:  
StartTime  
Duration  
SetType
