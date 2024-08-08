﻿namespace BookOnline.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> AddAsync(Author author);

        Task<Response<Author>> GetByIDAsync(int? id);

        Author Update(Author author);
        Author DeleteAuthor(Author author);
    }
}
