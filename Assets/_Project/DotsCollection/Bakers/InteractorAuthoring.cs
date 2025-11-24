namespace DarkLordGame
{
    public class InteractorAuthoring : StructAuthorizer<Interactor>
    {
    }

    public class InteractorBaker : StructBaker<InteractorAuthoring, Interactor> { }

}
