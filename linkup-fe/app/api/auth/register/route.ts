import { type NextRequest, NextResponse } from "next/server"
import bcrypt from "bcryptjs"
import { sql } from "@/lib/database"

export const dynamic = "force-dynamic"

export async function POST(request: NextRequest) {
  try {
    const body = await request.json()
    const { firstName, lastName, username, email, password } = body

    if (!firstName || !lastName || !username || !email || !password) {
      return NextResponse.json({ error: "Todos los campos son requeridos" }, { status: 400 })
    }

    // Check if user already exists
    const existingUser = await sql`
      SELECT id FROM users WHERE email = ${email} OR user_name = ${username}
    `

    if (existingUser.length > 0) {
      return NextResponse.json({ error: "El usuario ya existe" }, { status: 400 })
    }

    // Hash password
    const hashedPassword = await bcrypt.hash(password, 12)

    // Create user
    const result = await sql`
      INSERT INTO users (
        first_name, 
        last_name, 
        user_name, 
        email, 
        password, 
        created_at
      ) VALUES (
        ${firstName},
        ${lastName},
        ${username},
        ${email},
        ${hashedPassword},
        CURRENT_TIMESTAMP
      )
      RETURNING id, email, user_name
    `

    return NextResponse.json(
      {
        message: "Usuario creado exitosamente",
        user: result[0],
      },
      { status: 201 },
    )
  } catch (error) {
    console.error("Error creating user:", error)
    return NextResponse.json({ error: "Error interno del servidor" }, { status: 500 })
  }
}
