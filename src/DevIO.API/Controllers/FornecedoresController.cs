using AutoMapper;
using DevIO.API.Dtos;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Controllers
{
    [Route("api/[controller]")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IFornecedorService fornecedorService,
                                      IEnderecoRepository enderecoRepository,
                                      IMapper mapper,
                                      INotificador notificador): base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _enderecoRepository = enderecoRepository;
            _notificador = notificador;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<FornecedorDto>> ObterTodos()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorDto>>(await _fornecedorRepository.ObterTodos());
            foreach (var fornecedor in fornecedores)
            {
               fornecedor.Endereco = _mapper.Map<EnderecoDto>(await _enderecoRepository.ObterEnderecoPorFornecedor(fornecedor.Id));    
            }
            return fornecedores;
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> ObterPorId(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutosEndereco(id);
            if(fornecedor == null) return NotFound();
            return fornecedor;
        }
        [HttpPost]
        public async Task<ActionResult<FornecedorDto>> Adicionar(FornecedorDto fornecedorDto)
        {
            if(!ModelState.IsValid) return BadRequest();
            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto);
            var result = await _fornecedorService.Adicionar(fornecedor);
            if(!result) return BadRequest();

            return Ok();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Atualizar(Guid id, FornecedorDto fornecedorDto)
        {
            if(id != fornecedorDto.Id) return BadRequest();
            if(!ModelState.IsValid) return BadRequest();
            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto);
            var result = await _fornecedorService.Atualizar(fornecedor);
            if(!result) return BadRequest();

            return Ok();
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Excluir(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            if(fornecedor == null) return NotFound();

            var resultado = await _fornecedorService.Remover(id);
            if(!resultado) return BadRequest();
            return Ok(fornecedor);
        }
        private async Task<FornecedorDto> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
        private async Task<FornecedorDto> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}