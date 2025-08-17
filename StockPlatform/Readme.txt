dto hm jb bnaty hen jb koi data hm nhi bhjna xhahty 

Dto flder create krlo 

// aur usme ek class create krlo jiska naam hoga "EmptyDto" ya koi bhi naam rakh lo

is my sarui model wali property hogi mgr us k  ilawa jo nhi bhejna chahty 


us k bad mapper bnao

public static class EntityMapper
{
    // Model -> DTO
public static class [ModelName]Mappers
{
    public static [DtoName] To[DtoName](this [ModelName] modelObject)
    {
        return new [DtoName]
        {
            Property1 = modelObject.Property1,
            Property2 = modelObject.Property2,
            // ... Add more as needed
        };
    }
}


    // DTO -> Model //jb frontend se data a ra ho
    public static class [ModelName]Mappers
{
    public static [ModelName] To[ModelName](this [DtoName] dtoObject)
    {
        return new [ModelName]
        {
            Property1 = dtoObject.Property1,
            Property2 = dtoObject.Property2,
            // ... Add more
        };
    }
}


Mapper ka Kaam Kya Hai?
 Mapper = bridge between Model <-> DTO
 Ye kaam tum manually karo ya AutoMapper se

 Large projects mein 
 AutoMapper time bachata hai


 then make changes in controllers see sontroller for details


 //
 Newtonsoft.Json //latest version
 Microsoft.AspNetCore.Mvc.NewtonsoftJson


 in program .cs: builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


Newtonsoft.Json circular references (jaise Stock → Comments → Stock) handle karne ke liye install karte hain.

Default JSON serializer (System.Text.Json) circular loop pe error deta hai.

Jab models mein navigation properties hoon, tab Newtonsoft.Json use karna chahiye.

ReferenceLoopHandling.Ignore se JSON infinite loop skip kar deta hai aur API crash nahi hoti.

Newtonsoft.Json circular references (jaise Stock → Comments → Stock) handle karne ke liye install karte hain.

Default JSON serializer (System.Text.Json) circular loop pe error deta hai.

Jab models mein navigation properties hoon, tab Newtonsoft.Json use karna chahiye.

ReferenceLoopHandling.Ignore se JSON infinite loop skip kar deta hai aur API crash nahi hoti.