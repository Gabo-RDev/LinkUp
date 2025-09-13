import { type NextRequest, NextResponse } from "next/server"
import { sql } from "@/lib/database"

export const dynamic = "force-dynamic"

export async function GET(request: NextRequest) {
  try {
    const { searchParams } = request.nextUrl
    const query = searchParams.get("q")
    const limit = Number.parseInt(searchParams.get("limit") || "5")

    if (!query || query.length < 2) {
      return NextResponse.json({ suggestions: [] })
    }

    // Search for posts and categories that match the query
    const [posts, categories] = await Promise.all([
      sql`
        SELECT id, title, 'post' as type
        FROM posts 
        WHERE (title ILIKE ${`%${query}%`} OR content ILIKE ${`%${query}%`}) 
        AND deleted = false
        ORDER BY created_at DESC
        LIMIT ${limit}
      `,
      sql`
        SELECT id, name as title, 'category' as type
        FROM posts_category 
        WHERE name ILIKE ${`%${query}%`}
        LIMIT 3
      `,
    ])

    const suggestions = [...categories, ...posts].slice(0, limit)

    return NextResponse.json({ suggestions })
  } catch (error) {
    console.error("Error searching:", error)
    return NextResponse.json({ error: "Search failed" }, { status: 500 })
  }
}
