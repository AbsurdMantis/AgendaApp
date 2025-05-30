﻿using AgendaApp.Server.DTOs;
using AgendaApp.Server.Models;
using AgendaApp.Server.Repositories;
using AutoMapper;
using FluentValidation;

namespace AgendaApp.Server.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IRabbitMQPublisher _publisher;
        private readonly IContatoRepository _repo;
        private readonly IMapper _mapper;
        private readonly IValidator<ContatoDTO> _validator;

        public ContatoService(IContatoRepository repo, IMapper mapper, IValidator<ContatoDTO> validator, IRabbitMQPublisher publisher)
        {
            _repo = repo;
            _mapper = mapper;
            _validator = validator;
            _publisher = publisher;
        }

        public async Task<ContatoDTO> AddContatoAsync(ContatoDTO contatoDTO)
        {
            _validator.ValidateAndThrow(contatoDTO);
            var entity = _mapper.Map<Contato>(contatoDTO);

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            await _publisher.PublishEvent(entity, ContatoEventType.ContatoCriado);

            return _mapper.Map<ContatoDTO>(entity);
        }

        public async Task<ContatoDTO?> GetContatoAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);

            return entity == null ? null : _mapper.Map<ContatoDTO>(entity);
        }

        public async Task<IEnumerable<ContatoDTO>> GetContatosAsync()
        {
            var entities = await _repo.GetAllAsync();

            return _mapper.Map<IEnumerable<ContatoDTO>>(entities);
        }

        public async Task<bool> RemoveContatoAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);

            //shortar aqui e add mensagem caso falso
            if (entity == null)
            {
                return default;
            }

            await _repo.DeleteAsync(entity);

            await _publisher.PublishEvent(entity, ContatoEventType.ContatoDeletado);

            return await _repo.SaveChangesAsync();
        }

        public async Task<ContatoDTO?> UpdateContatoAsync(int id, ContatoDTO contatoDTO)
        {
            _validator.ValidateAndThrow(contatoDTO);
            var checkEntity = await _repo.GetByIdAsync(id);

            if (checkEntity == null)
            {
                return default;
            }

            checkEntity.Nome = contatoDTO.Nome;
            checkEntity.Email = contatoDTO.Email;
            checkEntity.Telefone = contatoDTO.Telefone;
            await _repo.UpdateAsync(checkEntity);
            await _publisher.PublishEvent(checkEntity, ContatoEventType.ContatoEditado);

            return _mapper.Map<ContatoDTO>(checkEntity);
        }
    }
}
