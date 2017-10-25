using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Palindrome_Modal
{
    class Program
    {
        static void Main()
        {
            try
            {
                //Lê o arquivo de entrada e retorna uma lista de string com cada linha do arquivo
                List<string> input = LerArquivoInput();

                //Monta a 'tabela de caracteres' do pdf a partir de um arquivo
                Dictionary<string, string> dicionario = MontarTabelaCaracteres();

                //Realiza as verificações de cada string de entrada e imprime os resultados
                VerificarInput(input, dicionario);

                Console.WriteLine("Pressione qualquer tecla para finalizar...");
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine("--Erro-- \n" + ex);
            }
        }

        private static StreamReader AbreArquivo(string arquivo)
        {
            //Faço Try catch para caso haja problema na abertura do arquivo.
            try
            {
                //retorna o arquivo passado como parâmetro
                return File.OpenText(arquivo);
            }
            catch (FileNotFoundException fnfex)
            {
                throw new Exception("\n--Arquivo não encontrado--\n" + fnfex);
            }
            catch (IOException ioex)
            {
                throw new Exception("\n--Erro ao abrir o arquivo--\n" + ioex);
            }
        }

        private static List<string> LerArquivoInput()
        {
            //chamo um método para abrir o arquivo passado como argumento
            StreamReader input = AbreArquivo("in.txt");

            //Cria uma lista de strings
            List<string> listaInputs = new List<string>();

            //Pego as linhas do arquivo de entrada separadamente, até o fim do arquivo
            while (!input.EndOfStream)
            {
                string linha = input.ReadLine();
                //adiciono a lista de strings
                listaInputs.Add(linha);
            }
            return listaInputs;
        }

        private static Dictionary<string, string> MontarTabelaCaracteres()
        {
            //chamo um método para abrir o arquivo passado como argumento
            StreamReader arquivoCaracteres = AbreArquivo("Table.txt");

            //Crio um dicionario para armazenar os caracteres e seus reversos
            Dictionary<string, string> dicionario = new Dictionary<string, string>();

            //Pego as linhas do arquivo da tabela separadamente, até o fim do arquivo
            while (!arquivoCaracteres.EndOfStream)
            {
                string linha = arquivoCaracteres.ReadLine();

                //Se a linha possuir tamanho maior que 1, quer dizer que possui reverso
                //então passo para o dicionario a primeira posicao da linha, que é o caracter, e a ultima posicao, que é o reverso.
                if (linha.Length > 1)
                    dicionario.Add(linha[0].ToString(), linha[linha.Length - 1].ToString());
                else//Se o tamanho nao for maior que 1, quer dizer que o caracter nao tem reverso 
                    dicionario.Add(linha, null);
            }
            return dicionario;
        }

        private static void VerificarInput(List<string> listaInput, Dictionary<string, string> dicionario)
        {
            bool ehPalindromo;
            bool ehStringEspelhada;

            //Faço um foreach na lista de string, pegando uma por uma e as passando para os métodos VerificaPalindromo e VerificaStringEspelhada.
            //Posteriormente pego os retornos booleanos desses métodos e os passo juntamente com a string do laço para o método Imprimir
            listaInput
                .ForEach(@string => Imprimir(@string,
                ehPalindromo = VerificarPalindromo(@string),
                ehStringEspelhada = VerificarStringEspelhada(@string, dicionario)
                ));
        }

        private static bool VerificarPalindromo(string stringEntrada)
        {
            //Posicao inicial e final da string
            int posicaoIncial = 0;
            int posicaoFinal = stringEntrada.Length - 1;

            //Faço com que a string de entrada receba ela mesma com as letras maiúsculas, para que no caso de
            //haver letras minúsculas no meio da string, não haja diferenciação na hora de comparar uma letra 
            //maiúscula(ex:'W') da string, com a mesma letra, só que na forma minúscula(ex:'w'). 
            stringEntrada = stringEntrada.ToUpper();

            //Laço que percorre até a posição central da string, verificando a igualdade dos caracteres
            while (posicaoIncial < posicaoFinal)
            {
                //Pega o valor das variáveis 'posicao',faz a comparação e depois as incrementa/decrementa
                if (stringEntrada[posicaoIncial++] != stringEntrada[posicaoFinal--])
                    return false;
            }
            return true;
        }

        private static bool VerificarStringEspelhada(string stringEntrada, Dictionary<string, string> dicionario)
        {
            int posicaoInicial = 0;
            int posicaoFinal = stringEntrada.Length - 1;

            //Faço com que a string de entrada receba ela mesma com as letras maiúsculas, para que no caso de
            //haver letras minúsculas no meio da string, não haja diferenciação na hora de comparar uma letra 
            //maiúscula(ex:'W') da string, com a mesma letra, só que na forma minúscula(ex:'w'). 
            stringEntrada = stringEntrada.ToUpper();

            //Crio um array de char a partir da string de entrada
            char[] entradaChar = stringEntrada.ToCharArray();

            //Laço que percorre a string de entrada até a posição central
            while (posicaoInicial < posicaoFinal)
            {
                //Pego um caracter da posicao inicial da string separadamente a cada iteração
                string caracter = entradaChar[posicaoInicial].ToString();

                //Seleciono no dicionario quem possui a chave igual ao caracter pego anteriormente
                //e seleciono seu valor(que no caso é seu caracter reverso).
                string caracterReverso = dicionario.Where(c => c.Key == caracter).Select(r => r.Value).First();

                if (caracterReverso != null)
                {   //Verifico se os caracteres das posições finais são correspondentes
                    //ao reverso do caracteres espelhados nas posições iniciais
                    if (stringEntrada[posicaoFinal].ToString() != caracterReverso)
                        return false;
                }
                else
                {   //Caso o caracter das posições inciais não possua um reverso, ele deve ser correspondente ao seu espelhado
                    //nas posições finais
                    if (stringEntrada[posicaoFinal].ToString() != caracter)
                        return false;
                }
                //Incremento a possição inicial e decremento a final para pegar os próximos respectivos caracteres
                posicaoInicial++;
                posicaoFinal--;
            }
            return true;
        }

        private static void Imprimir(string @string, bool ehPalindromo, bool ehStringEspelhada)
        {
            //Verifico o valor das variáveis booleanas para definir a mensangem que será enviada
            //Uso '\n' para criar a linha em branco sem ter que chamar a função novamente
            if (!ehPalindromo && !ehStringEspelhada)
                Console.WriteLine(@string + " -- is not a palindrome.\n");

            else if (ehPalindromo && !ehStringEspelhada)
                Console.WriteLine(@string + " -- is a regular palindrome.\n");

            else if (!ehPalindromo && ehStringEspelhada)
                Console.WriteLine(@string + " -- is a mirrored string.\n");

            else
                Console.WriteLine(@string + " -- is a mirrored palindrome.\n");
        }
    }
}
