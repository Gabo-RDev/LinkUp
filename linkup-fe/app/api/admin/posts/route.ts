import { type NextRequest, NextResponse } from "next/server"
import { getAdminPosts, createPost } from "@/lib/admin-database"

export const dynamic = "force-dynamic"

export async function GET(request: NextRequest) {
  try {
    const { searchParams } = request.nextUrl
    const page = Number.parseInt(searchParams.get("page") || "1")
    const limit = Number.parseInt(searchParams.get("limit") || "20")
    const adminId = searchParams.get("adminId") || undefined

    const offset = (page - 1) * limit
    const posts = await getAdminPosts(adminId, limit, offset)

    return NextResponse.json({
      posts,
      pagination: {
        page,
        limit,
        hasMore: posts.length === limit,
      },
    })
  } catch (error) {
    console.error("Error fetching admin posts:", error)
    return NextResponse.json({ error: "Failed to fetch posts" }, { status: 500 })
  }
}

export async function POST(request: NextRequest) {
  try {
    const body = await request.json()
    const { title, content, categoryId, adminId } = body

    if (!title || !content || !categoryId || !adminId) {
      return NextResponse.json({ error: "Missing required fields" }, { status: 400 })
    }

    const post = await createPost({
      title,
      content,
      categoryId,
      adminId,
    })

    return NextResponse.json({ post }, { status: 201 })
  } catch (error) {
    console.error("Error creating post:", error)
    return NextResponse.json({ error: "Failed to create post" }, { status: 500 })
  }
}
