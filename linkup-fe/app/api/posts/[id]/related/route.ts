import { type NextRequest, NextResponse } from "next/server"
import { sql } from "@/lib/database"

export const dynamic = "force-dynamic"

export async function GET(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id } = params

    if (!id) {
      return NextResponse.json({ error: "Post ID is required" }, { status: 400 })
    }

    // Get related posts based on same category or similar interests
    const relatedPosts = await sql`
      SELECT DISTINCT
        p.id,
        p.title,
        p.content,
        p.likes_count,
        p.created_at,
        pc.name as category_name,
        pc.id as category_id,
        a.first_name || ' ' || a.last_name as author_name,
        a.user_name as author_username,
        a.profile_photo as author_photo
      FROM posts p
      LEFT JOIN posts_category pc ON p.category_id = pc.id
      LEFT JOIN admins a ON p.admin_id = a.id
      WHERE p.id != ${id} 
        AND p.deleted = false
        AND p.category_id = (
          SELECT category_id FROM posts WHERE id = ${id}
        )
      ORDER BY p.created_at DESC
      LIMIT 4
    `

    return NextResponse.json({ relatedPosts })
  } catch (error) {
    console.error("Error fetching related posts:", error)
    return NextResponse.json({ error: "Failed to fetch related posts" }, { status: 500 })
  }
}
