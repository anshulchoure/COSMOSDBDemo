using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts2TableCosmosMVC.Models.Abstract;
using Contacts2TableCosmosMVC.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Contacts2TableCosmosMVC.Models.Concrete
{
  public class TableContactRepository : IContactRepository
  {
    private readonly ILogger<TableContactRepository> _logger;
    private readonly CloudStorageAccount _cloudStorageAccount;
    private readonly CloudTableClient _cloudTableClient;
    private readonly CloudTable _cloudTable;

    public TableContactRepository(IOptions<StorageUtility> storageUtility, ILogger<TableContactRepository> logger)
    {
      _logger = logger;
      _cloudStorageAccount = storageUtility.Value.StorageAccount;
      _cloudTableClient = _cloudStorageAccount.CreateCloudTableClient();
      _cloudTable = _cloudTableClient.GetTableReference("Contacts");
      _cloudTable.CreateIfNotExistsAsync().GetAwaiter().GetResult();
    }

    private ContactTable SetContactTableObject(Contact contact)
    {
      contact.Id = Guid.NewGuid().ToString();
      ContactTable contactTable = new ContactTable
      {
        Id = contact.Id,
        PartitionKey = contact.ContactName,
        RowKey = contact.Phone,
        ContactType = contact.ContactType,
        Email = contact.Email
      };
      return contactTable;
    }

    private Contact SetContactObject(ContactTable contactTable)
    {
      Contact contact = new Contact
      {
        Id = contactTable.Id,
        ContactName = contactTable.PartitionKey,
        Phone = contactTable.RowKey,
        ContactType = contactTable.ContactType,
        Email = contactTable.Email
      };
      return contact;
    }

    private List<Contact> SetContactsList(List<ContactTable> contactTableList)
    {
      return null;
    }
    public async Task<Contact> CreateAsync(Contact contact)
    {
      return null;
    }

    public async Task DeleteAsync(string id, string contactName, string phone)
    {

    }

    public async Task<Contact> FindContactAsync(string id)
    {
      return null;
    }

    public async Task<List<Contact>> FindContactByPhoneAsync(string phone)
    {
      return null;
    }

    public async Task<List<Contact>> FindContactCPAsync(string contactName, string phone)
    {
      return null;
    }

    public async Task<List<Contact>> FindContactsByContactNameAsync(string contactName)
    {
      return null;
    }

    public async Task<List<Contact>> GetAllContactsAsync()
    {
      return null;
    }

    public async Task<Contact> UpdateAsync(Contact contact)
    {
      var contactTable = SetContactTableObject(contact);
      TableOperation retrieveOperation = TableOperation.Retrieve<ContactTable>(contactTable.PartitionKey, contactTable.RowKey);
      TableResult tableResult = await _cloudTable.ExecuteAsync(retrieveOperation);
      var contactToUpdate = tableResult.Result as ContactTable;
      contactToUpdate.ContactType = contactTable.ContactType;
      contactToUpdate.Email = contactTable.Email;
      if (contactToUpdate != null)
      {
        TableOperation updateContact = TableOperation.Replace(contactToUpdate);
        var updateResult = await _cloudTable.ExecuteAsync(updateContact);
        var contactTableResult = updateResult.Result as ContactTable;
        return SetContactObject(contactTableResult);
      }
      return null;
    }
  }
}
