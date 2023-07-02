using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        // <summary>
        // Represents business logic for munipulating Person Entity
        // <return>
        // return the PersonResponse type of object
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        // <summary>
        // return all the Person in current list
        // <return>
        // return a list of PersonResponse object
        List<PersonResponse> GetAllPersons();

        // <summary>
        // Returns person by PersonID
        // <return>
        // Return a PersonResponse object by parameter guid personID
        PersonResponse? GetPersonByPersonID(Guid? personID);
    }
}
