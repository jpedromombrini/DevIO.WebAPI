using AutoMapper;
using DevIO.API.Dtos;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace DevIO.API.Controllers
{
    [Authorize]
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
            _mapper = mapper;
        }
        [AllowAnonymous]
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
            if(!ModelState.IsValid) return CustomResponse(ModelState);              
            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorDto));
            return CustomResponse(fornecedorDto);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Atualizar(Guid id, FornecedorDto fornecedorDto)
        {
            if(id != fornecedorDto.Id)
            {            
                NotificarErro("O id informado não é o mesmo Que foi passado na query");
                return CustomResponse(fornecedorDto);
            } 
            if(!ModelState.IsValid) return CustomResponse(ModelState);            
            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorDto));
            return CustomResponse(fornecedorDto);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Excluir(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            if(fornecedor == null) return NotFound();
            await _fornecedorService.Remover(id);            
            return CustomResponse();
        }
        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<EnderecoDto> ObterEndereco(Guid id)
        {
            var enderecoDto = _mapper.Map<EnderecoDto>(await _enderecoRepository.ObterPorId(id));
            return enderecoDto;
        }
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoDto enderecoDto)
        {
             if(id != enderecoDto.Id)
            {            
                NotificarErro("O id informado não é o mesmo Que foi passado na query");
                return CustomResponse(enderecoDto);
            } 
            if(!ModelState.IsValid) return CustomResponse(ModelState);            
            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoDto));
            return CustomResponse(enderecoDto);
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