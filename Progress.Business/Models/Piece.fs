﻿namespace Business.Models

open System

type public GetPiece = {
    Id : Guid
    Name: string
    }

type public AddPiece = {
    Name: string
    }