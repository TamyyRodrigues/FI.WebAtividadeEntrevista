var listaObjetos = [];
$(document).ready(function () {
    $('#formBeneficiarioModal').submit(function (e) {
        e.preventDefault();

        var nome = $('#NomeBeneficiario').val();
        var cpf = $('#CPFBeneficiario').val();

        var dados = {
            "model": {
                "NOME": nome,
                "CPF": cpf
            }
        };

        $.ajax({
            url: urlArmazenarBeneficiario,
            method: "POST",
            contentType: 'application/json',
            data: JSON.stringify(dados),
            error: function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor");
            },
            success: function (r) {
                $('#formBeneficiarioModal')[0].reset();
                listaObjetos = r.beneficiarios;
                atualizarGrid(r.beneficiarios);

            }
        });
    });
});

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
};

function atualizarGrid(beneficiarios) {
    var tbody = $('#listaBeneficiarios');
    tbody.empty();
    beneficiarios.forEach(function (beneficiario, index) {
        var row = $('<tr>');
        row.append($('<td>').text(beneficiario.Nome));
        row.append($('<td>').text(beneficiario.CPF));

        var btnAlterar = $('<button>').text('Alterar').addClass('btn btn-primary btn-sm ');
        var btnExcluir = $('<button>').text('Excluir').addClass('btn btn-primary btn-sm');

        btnAlterar.click(function () {
            $('#NomeBeneficiario').val(beneficiario.Nome);
            $('#CPFBeneficiario').val(beneficiario.CPF);

            Remover(beneficiario.CPF);
            beneficiarios.splice(index, 1);
            row.remove();
        });

        btnExcluir.click(function () {
            Remover(beneficiario.CPF);
            beneficiarios.splice(index, 1);
            row.remove();

            $('#formBeneficiarioModal')[0].reset();
        });

        var space = $('<span>').html('&nbsp;&nbsp;');

        row.append($('<td>').append(btnAlterar).append(space).append(btnExcluir));
        tbody.append(row);
    });
}

function Remover(CPF) {
    var dados = {
        "CPF": CPF
    };
    $.ajax({
        url: urlRemoverBeneficiario,
        method: "POST",
        contentType: 'application/json',
        data: JSON.stringify(dados),
        error: function (r) {
            if (r.status == 400)
                ModalDialog("Ocorreu um erro", r.responseJSON);
            else if (r.status == 500)
                ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor");
        },
        success: function (r) {
            atualizarGrid(r.beneficiarios);

        }
    });
}