# 🧠 TesseractOCR - Sistema de OCR Assíncrono com AWS SQS e MongoDB

Este projeto é uma API .NET que realiza o processamento de imagens via OCR (Reconhecimento Óptico de Caracteres) utilizando o Tesseract. A arquitetura segue o padrão **hexagonal**, com comunicação assíncrona por meio do **Amazon SQS** e armazenamento dos resultados em **MongoDB**.

---

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core** – API REST.
- **Tesseract OCR** – Biblioteca para extração de texto de imagens.
- **Amazon SQS (Simple Queue Service)** – Mensageria assíncrona.
- **MongoDB** – Banco de dados NoSQL para persistência dos resultados.
- **Docker** – Execução do MongoDB localmente.
- **SixLabors.ImageSharp** – Manipulação de imagens em memória.
- **Arquitetura Hexagonal (Ports & Adapters)** – Organização em camadas isoladas.

---

## 📦 Estrutura da Solução

TesseractOCR/
  │
  ├── Api
  ├── Application
  ├── Infrastructure
  ├── Domain
  └── TesseractOCR.csproj 
  
---

## 🔄 Fluxo da Aplicação

1. **Envio de Imagem (Controller)**  
   O cliente envia uma imagem ou arquivo que é convertida em **Base64 PNG** via endpoint HTTP.

2. **Publicação na Fila (Amazon SQS)**  
   A imagem é serializada e enviada para uma fila do SQS.

3. **Consumer (Worker)**  
   Um serviço worker escuta mensagens da fila de forma contínua. Ao receber:
   - Converte a imagem Base64 em `Stream`.
   - Processa a imagem com o **Tesseract** para extrair o texto.
   - Gera um DTO com o resultado (texto, confiança, bounding boxes).

4. **Persistência (MongoDB)**  
   O resultado do OCR é armazenado em uma coleção no MongoDB para futura consulta.

---

## 🧪 Endpoints Disponíveis

| Método | Rota                           | Descrição                                    |
|--------|--------------------------------|----------------------------------------------|
| POST   | `/api/TesseractOcr/`           | Envia imagem para processamento via SQS      |
| GET    | `/api/TesseractOcr/`           | Retorna os documentos processados do MongoDB |


