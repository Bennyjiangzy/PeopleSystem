using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        /// <summary>
        /// Represents business logic for munipulating Person Entity
        /// <return>
        /// return the PersonResponse type of object
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// return all the Person in current list
        /// <return>
        /// return a list of PersonResponse object
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns person by PersonID
        /// </summary>
        /// <param name ="personID">Person ID to search</param>
        /// <return>Return a PersonResponse object by parameter guid personID </return>
        PersonResponse? GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns all person objects that matched with the given search field and search string
        /// </summary>
        /// <param name ="searchBy">Search field to search</param>
        /// <param name ="searchString">Search string to search</param>
        /// <return>Returns all matching persons based on the given search field and search string </return>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
    }
}
