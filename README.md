# Test infinity puzzle
Projeto desenvolvido como desafio para infinity games.

Meu direcionamento para este projeto foi imaginar uma situação de trabalho, onde é apresentado o documento e é solicitado uma demonstração das possibilidades para tal lógica de jogo. Acredito que com o atual estágio da aplicação seria possível apontarmos melhorias na lógica de jogo e funcionamento da aplicação, para um possível lançamento. Para o atual estágio, demorei por volta de 12 horas de desenvolvimento, entre estudos, implementação e testes.

Para este projeto tomei como base o descrito no documento entregue. Se trata de um puzzle, onde peças são espalhadas num grid, e o usuário deve colocá-las na posição correta. Criei algumas regras adicionais de jogo, como não randomizar a posição de espaços em branco e peças com uma ou quatro conexões, fazendo o gameplay ser um pouco mais fácil.
Outra regra que adotei é que ao colocar uma peça em determinada posição, caso a posição esteja correta, troca de posição com a peça que já estava naquela posição, evitando ter peças sobrepostas. Importante salientar que tomei como base peças quadradas, e não hexagonais como no Loop Energy, para simplificação da lógica do jogo.

Sobre o código, tive que fazer algumas pesquisas sobre método de gerar este tipo de puzzle, com peças variando em possibilidades de conexão. Após isso, gerei a lógica para identificação de posição e troca de posições. Por fim, fiz uma interface básica, para apresentar os meus dominios em Unity, com UI, particulas e etc.

A aplicação foi testada em iPhone, e em um simulador de Android (não disponho de um aparelho Android neste momento).
